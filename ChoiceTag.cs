using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceTag : MonoBehaviour
{
    //菜單Item Tag物件
    private GameObject MenuTag_Item;
    private GameObject MenuTag_Save;
    private GameObject MenuTag_Exit;

    //菜單Item Tag動畫組件
    private Animator itemAnimator;
    private Animator saveAnimator;
    private Animator exitAnimator;

    //菜單物件
    private GameObject MenuObject;

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

    void Start()
    {
        //獲取菜單各Tag物件及其動畫組件
        MenuTag_Item = GameObject.Find("MenuTag_Item");
        MenuTag_Save = GameObject.Find("MenuTag_Save");
        MenuTag_Exit = GameObject.Find("MenuTag_Exit");
        MenuObject = GameObject.Find("Menu");
        itemAnimator = MenuTag_Item.GetComponent<Animator>();
        saveAnimator = MenuTag_Save.GetComponent<Animator>();
        exitAnimator = MenuTag_Exit.GetComponent<Animator>();
        tagIndex = 1;//初始化當前位置編號
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        float cancel = Input.GetAxisRaw("Cancel");//檢測x鍵
        restTimer += Time.deltaTime;
        doMove(v);
        doSubmit(submit);
        if (PlayerItemMenu.openDetailMenu)
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
                Vector3 targetPosition = MenuTag_Item.transform.position;
                transform.position = new Vector3(targetPosition.x + 0.25F, targetPosition.y + 0.05F, targetPosition.z);
                setStatus("item");
            }
            else if (tagIndex == 2)
            {
                Vector3 targetPosition = MenuTag_Save.transform.position;
                transform.position = new Vector3(targetPosition.x + 0.25F, targetPosition.y + 0.05F, targetPosition.z);
                setStatus("save");
            }
            restTimer = 0;
        }
        else if (v < 0 && tagIndex <= tagIndexMax && overRestTime)//向下移動
        {
            tagIndex++;
            if (tagIndex == 2)
            {
                Vector3 targetPosition = MenuTag_Save.transform.position;
                transform.position = new Vector3(targetPosition.x + 0.25F, targetPosition.y + 0.05F, targetPosition.z);
                setStatus("save");
            }
            else if (tagIndex == 3)
            {
                Vector3 targetPosition = MenuTag_Exit.transform.position;
                transform.position = new Vector3(targetPosition.x + 0.25F, targetPosition.y + 0.05F, targetPosition.z);
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
                    MenuTag_Item.SendMessage("showItemPage");
                    restTimer = 0;
                    break;
                case 2:
                    MenuTag_Save.SendMessage("showSavePage");
                    restTimer = 0;
                    break;
                case 3:
                    MenuTag_Exit.SendMessage("showExitPage");
                    restTimer = 0;
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
            restTimer = 0;
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


}
