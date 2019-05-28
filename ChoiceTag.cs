using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceTag : MonoBehaviour
{
    //菜單Item Tag動畫組件
    private Animator itemAnimator;
    private Animator saveAnimator;
    private Animator exitAnimator;

    //菜單物件
    private GameObject MenuObject;

    //菜單Page物件
    private GameObject ItemPage;
    private GameObject SavePage;
    private GameObject ExitPage;

    //當前位置編號
    private int tagIndex;

    //最大位置編號
    private int tagIndexMax = 3;

    [Header("切換菜單tag應休息時間")]
    [Range(0F, 5F)]
    public float restTime = 0.5F;

    //切換菜單tag已休息時間
    private float restTimer = 5F;

    //等待時間是否超過應等待時間
    private bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    /*
     * 打開二級菜單類型
     * -1 --> 未打開一級菜單
     * 0 --> 未打開二級菜單
     * 1 --> Item
     * 2 --> Save
     * 3 --> Exit
     */
    public static int openDetailMenu;

    void Start()
    {
        //獲取菜單物件及其動畫組件
        MenuObject = GameObject.Find("Menu");
        itemAnimator = GameObject.Find("MenuTag_Item").GetComponent<Animator>();
        saveAnimator = GameObject.Find("MenuTag_Save").GetComponent<Animator>();
        exitAnimator = GameObject.Find("MenuTag_Exit").GetComponent<Animator>();
        //獲取菜單各page物件
        ItemPage = GameObject.Find("ItemPage");
        SavePage = GameObject.Find("SavePage");
        ExitPage = GameObject.Find("ExitPage");
        tagIndex = 1;//初始化當前位置編號
        openDetailMenu = 0;//剛開始未打開二級菜單
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        float cancel = Input.GetAxisRaw("Cancel");//檢測x鍵
        restTimer += Time.deltaTime;
        if (openDetailMenu == 0)
        {
            doMove(v);
            doSubmit(submit);
        }
        else
        {
            doCancel(cancel);
        }
    }

    //菜單選擇器的移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 0 && overRestTime)//向上移動
        {
            tagIndex--;
            if (tagIndex == 1)
            {
                setStatus("item");
            }
            else if (tagIndex == 2)
            {
                setStatus("save");
            }
            restTimer = 0;
        }
        else if (v < 0 && tagIndex <= tagIndexMax && overRestTime)//向下移動
        {
            tagIndex++;
            if (tagIndex == 2)
            {
                setStatus("save");
            }
            else if (tagIndex == 3)
            {
                setStatus("exit");
            }
            restTimer = 0;
        }
    }

    //菜單選擇器的確認
    private void doSubmit(float submit)
    {
        if (submit > 0 && overRestTime)
        {
            MenuObject.SendMessage("doOpenDetailMenu");
            switch (tagIndex)
            {
                case 1:
                    ItemPage.SendMessage("showItemPage");
                    openDetailMenu = 1;
                    returnRestTimer();
                    break;
                case 2:
                    SavePage.SendMessage("showSavePage");
                    openDetailMenu = 2;
                    returnRestTimer();
                    break;
                case 3:
                    ExitPage.SendMessage("showExitPage");
                    ExitPage.SendMessage("returnRestTimer");
                    openDetailMenu = 3;
                    returnRestTimer();
                    break;
            }
        }
    }

    //開始二級菜單時的取消操作
    private void doCancel(float cancel)
    {
        if (cancel > 0 && overRestTime)
        {
            MenuObject.SendMessage("doCloseDetailMenu");
            switch (tagIndex)
            {
                case 1:
                    ItemPage.SendMessage("hideItemPage");
                    openDetailMenu = 0;
                    returnRestTimer();
                    break;
                case 2:
                    SavePage.SendMessage("hideSavePage");
                    openDetailMenu = 0;
                    returnRestTimer();
                    break;
                case 3:
                    ExitPage.SendMessage("hideExitPage");
                    openDetailMenu = 0;
                    returnRestTimer();
                    break;
            }
            GameObject.Find("Player").SendMessage("returnRestTimer");
        }
    }

    //設定批次狀態機的狀態
    private void setStatus(string status)
    {
        itemAnimator.GetComponent<Animator>().SetBool("choseTag", false);
        itemAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", false);
        saveAnimator.GetComponent<Animator>().SetBool("choseTag", false);
        saveAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", false);
        exitAnimator.GetComponent<Animator>().SetBool("choseTag", false);
        exitAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", false);
        if (status.Equals("item"))
        {
            itemAnimator.GetComponent<Animator>().SetBool("choseTag", true);
            saveAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
            exitAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
        else if (status.Equals("save"))
        {
            saveAnimator.GetComponent<Animator>().SetBool("choseTag", true);
            itemAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
            exitAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
        else if (status.Equals("exit"))
        {
            exitAnimator.GetComponent<Animator>().SetBool("choseTag", true);
            itemAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
            saveAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
    }

    //已休息時間歸零
    private void returnRestTimer()
    {
        restTimer = 0F;
    }

}
