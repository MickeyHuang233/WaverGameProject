﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    玩家菜單欄
    掛載物件：
        PlayerItemMenu --> Menu
            MenuTag
            Page
    實現功能：
        1. 控制菜單的層級顯示
*/
public class PlayerItemMenu : MonoBehaviour
{
    //玩家物件
    private GameObject playerObject;

    //菜單的動畫信息
    private Animator animator;

    /*
     * 菜單的Tag物件
     * 0 --> MenuTag_Item
     * 1 --> MenuTag_Save
     * 2 --> MenuTag_Exit
     */
    private List<GameObject> MenuTagsObject = new List<GameObject>();

    //菜單Page物件
    private GameObject ItemPage;
    private GameObject SavePage;
    private GameObject ExitPage;

    //菜單的Tag動畫組件
    private List<Animator> MenuTagsAnimator = new List<Animator>();

    /*
     * 打開二級菜單類型
     * -1 --> 未打開一級菜單
     * 0 --> 打開一級菜單，未打開二級菜單
     * 1 --> Item
     * 2 --> Save
     * 3 --> Exit
     */
    public static int openDetailMenu = -1;

    //當前位置編號
    private int tagIndex;

    //最大位置編號
    private int tagIndexMax = 3;

    [Header("切換菜單tag應休息時間")]
    [Range(0F, 5F)]
    public float setRestTime = 0.5F;

    //菜單所有操作的應休息時間
    public static float restTime;

    //切換菜單tag已休息時間
    public static float restTimer = 5F;

    //等待時間是否超過應等待時間
    public static bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    #region 已休息時間歸零
    public static void returnRestTimer()
    {
        restTimer = 0F;
    }
    #endregion

    #region Start()
    void Start()
    {
        //將設定的應休息時間放至靜態變量，以方便調用
        restTime = setRestTime;
        animator = GetComponent<Animator>();
        //設定菜單動畫狀態
        animator.SetBool("openMenu", false);
        animator.SetBool("closeMenu", true);
        //獲取菜單各page物件
        ItemPage = GameObject.Find("ItemPage");
        SavePage = GameObject.Find("SavePage");
        ExitPage = GameObject.Find("ExitPage");
        //取得菜單的Tag物件及動畫組件
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject t = transform.GetChild(0).GetChild(i).gameObject;
            MenuTagsObject.Add(t);
            Animator a = t.GetComponent<Animator>();
            MenuTagsAnimator.Add(a);
            //設定菜單下Tag動畫狀態
            a.SetBool("choseTag", false);
            a.SetBool("cancelChoseTag", false);
            a.SetBool("openMenu", false);
            a.SetBool("closeMenu", false);
        }
        tagIndex = 1;//初始化當前位置編號
        //取得玩家物件
        playerObject = GameObject.Find("Player");
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        restTimer += Time.deltaTime;
        if (openDetailMenu == 0)//當開啟一級菜單
        {
            if (Input.GetButtonDown("Vertical")) doFirstMenuMove(v);
            if (Input.GetButtonDown("Submit") && overRestTime) doOpenDetailMenu();
            if (Input.GetButtonDown("Cancel") && overRestTime) doCloseMenu();
        }
        if(openDetailMenu > 0)//當開啟任一個二級菜單
        {
            if (Input.GetButtonDown("Cancel") && overRestTime) doCloseDetailMenu();
        }
    }
    #endregion

    #region 一級菜單選擇器的移動  doFirstMenuMove(float v)
    private void doFirstMenuMove(float v)
    {
        if (v > 0 && tagIndex >= 1)//向上移動
        {
            tagIndex--;
            if (tagIndex == 1)  setTagStatus("item");
            else if (tagIndex == 2) setTagStatus("save");
            returnRestTimer();
        }
        else if (v < 0 && tagIndex < tagIndexMax)//向下移動
        {
            tagIndex++;
            if (tagIndex == 2) setTagStatus("save");
            else if (tagIndex == 3) setTagStatus("exit");
            returnRestTimer();
        }
    }
    #endregion

    #region 打開一級菜單  doOpenMenu()
    private void doOpenMenu()
    {
        playerObject.SendMessage("hideTalkBubble");//清空玩家頭上的對話泡泡狀態
        GameObject.Find("talkColliderObject").transform.localScale = new Vector3(0F, 0F, 0F);
        animator.SetBool("openMenu", true);
        animator.SetBool("closeMenu", false);
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", true);
            MenuTagsAnimator[i].SetBool("closeMenu", false);
            MenuTagsAnimator[i].SetBool("choseTag", false);
            MenuTagsAnimator[i].SetBool("cancelChoseTag", true);
            if (i == 0)//選擇第一個Tag
            {
                MenuTagsAnimator[i].SetBool("choseTag", true);
                MenuTagsAnimator[i].SetBool("cancelChoseTag", false);
            }
        }
        openDetailMenu = 0;
        returnRestTimer();
    }
    #endregion

    #region 關閉一級菜單  doCloseMenu()
    private void doCloseMenu()
    {
        GameObject.Find("talkColliderObject").transform.localScale = new Vector3(1F, 1F, 1F);
        animator.SetBool("openMenu", false);
        animator.SetBool("closeMenu", true);
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", false);
            MenuTagsAnimator[i].SetBool("closeMenu", true);
            MenuTagsAnimator[i].SetBool("choseTag", false);
            MenuTagsAnimator[i].SetBool("cancelChoseTag", true);
        }
        openDetailMenu = -1;
        tagIndex = 1;
        returnRestTimer();
        PlayerMovement.returnRestTimer();
    }
    #endregion

    #region 打開二級菜單  doOpenDetailMenu()
    private void doOpenDetailMenu()
    {
        //設置Menu動畫
        animator.SetBool("openDetilMenu", true);
        animator.SetBool("closeDetilMenu", false);
        //批次設置Tag動畫
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openDetilMenu", true);
            MenuTagsAnimator[i].SetBool("closeDetilMenu", false);
        }
        switch (tagIndex)
        {
            case 1:
                ItemPage.SendMessage("showItemPage");
                openDetailMenu = 1;
                break;
            case 2:
                SavePage.SendMessage("showSavePage");
                SavePage.SendMessage("showSaveInformation");
                SavePage.SendMessage("showSituactionTarget");
                openDetailMenu = 2;
                break;
            case 3:
                ExitPage.SendMessage("showExitPage");
                openDetailMenu = 3;
                break;
        }
        returnRestTimer();
    }
    #endregion

    #region 關閉二級菜單  doCloseDetailMenu()
    private void doCloseDetailMenu()
    {
        //設置Menu動畫
        animator.SetBool("openDetilMenu", false);
        animator.SetBool("closeDetilMenu", true);
        //批次設置Tag動畫
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openDetilMenu", false);
            MenuTagsAnimator[i].SetBool("closeDetilMenu", true);
        }
        switch (tagIndex)
        {
            case 1:
                ItemPage.SendMessage("hideItemPage");
                break;
            case 2:
                SavePage.SendMessage("hideSavePage");
                break;
            case 3:
                ExitPage.SendMessage("hideExitPage");
                break;
        }
        returnRestTimer();
        openDetailMenu = 0;
    }
    #endregion

    #region 設定批次Tag狀態機的狀態  setTagStatus(string status)
    private void setTagStatus(string status)
    {
        foreach (Animator menuTagsAnimator in MenuTagsAnimator)
        {
            menuTagsAnimator.GetComponent<Animator>().SetBool("choseTag", false);
            menuTagsAnimator.GetComponent<Animator>().SetBool("cancelChoseTag", false);
        }
        if (status.Equals("item"))
        {
            MenuTagsAnimator[0].GetComponent<Animator>().SetBool("choseTag", true);
            MenuTagsAnimator[1].GetComponent<Animator>().SetBool("cancelChoseTag", true);
            MenuTagsAnimator[2].GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
        else if (status.Equals("save"))
        {
            MenuTagsAnimator[1].GetComponent<Animator>().SetBool("choseTag", true);
            MenuTagsAnimator[0].GetComponent<Animator>().SetBool("cancelChoseTag", true);
            MenuTagsAnimator[2].GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
        else if (status.Equals("exit"))
        {
            MenuTagsAnimator[2].GetComponent<Animator>().SetBool("choseTag", true);
            MenuTagsAnimator[0].GetComponent<Animator>().SetBool("cancelChoseTag", true);
            MenuTagsAnimator[1].GetComponent<Animator>().SetBool("cancelChoseTag", true);
        }
    }
    #endregion
}
