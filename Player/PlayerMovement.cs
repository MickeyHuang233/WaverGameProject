﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    控制主角移動及傳送
    掛載物件：
        PlayerMovement --> Player(命名固定)
        SurveyJudge --> (碰撞器物件必須放在玩家物件底下的第一個)
    實現功能：
        1. 角色的拸動及動畫轉換// TODO 缺斜向四個角的動畫轉換
        2. 在角色上下移動時需要同時改動Z軸 // TODO 因程式重構，需要從history中取之前寫好的部分
        3. 按下確認鍵調查或對話，調用Player底下的碰撞器物件的方法(碰撞器物件必須放在玩家物件底下的第一個)
        4. 按下取消鍵開啟或關閉一級菜單，調用Player底下的菜單物件的方法(菜單物件必須放在玩家物件底下的第四個)
*/
public class PlayerMovement : MonoBehaviour
{
    //先取得目前動畫狀態的資訊
    AnimatorStateInfo currentState;

    //角色的鋼體屬性
    private Rigidbody2D rigidbody;

    //角色的動畫信息
    private Animator animator;

    //玩家應該的狀態
    private string shouldStatus;

    //上一幀角色方向
    private Vector2 preMoveDirction;

    //當前角色方向
    private Vector2 moveDirction;

    //對話域物件
    private GameObject talkColliderObject;

    //上一幀Y軸位置
    private float preYaxis;

    //當前幀Y軸位置
    private float nowYaxis;

    [Header("角色行走速度_縱向")]
    [Range(0.5F, 20F)]
    public float speedV = 5F;

    [Header("角色行走速度_橫向")]
    [Range(0.5F, 20F)]
    public float speedH = 0.1F;

    [Header("角色行走速度_Z")]
    [Range(-1F, 1F)]
    public float speedZ = -0.05F;

    [Header("調查用的物件位置所乘倍率_X")]
    [Range(-1F, 1F)]
    public float talkColliderPositionRateX = 0.1F;

    [Header("調查用的物件位置所乘倍率_Y")]
    [Range(-1F, 1F)]
    public float talkColliderPositionRateY = 0.1F;

    [Header("對話完成或關閉菜單所需等待時間")]
    [Range(0F, 5F)]
    public float restTime = 0.51F;

    //計算玩家已休息時間
    private float restTimer = 5F;

    //等待時間是否超過應等待時間
    private bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    #region 已休息時間歸零
    private void returnRestTimer()
    {
        restTimer = 0F;
    }
    #endregion

    //菜單物件
    private GameObject MenuObject;

    #region Start()
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        MenuObject = this.transform.GetChild(3).gameObject;
        nowYaxis = this.transform.position.y;//初始化當前幀的y軸位置
        preYaxis = this.transform.position.y;//初始化上一幀的y軸位置
        talkColliderObject = this.transform.GetChild(0).gameObject;//取得玩家物件下的對話域物件的碰撞器元件
        shouldStatus = "idle";
    }
    #endregion

    #region Update()
    void Update()
    {
        currentState = this.animator.GetCurrentAnimatorStateInfo(0);//取得當前動畫狀態的hashCode
        float h = Input.GetAxisRaw("Horizontal");//檢測水平移動
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        float cancel = Input.GetAxisRaw("Cancel");//檢測x鍵
        doSetStatusToShould(currentState);
        restTimer += Time.deltaTime;
        if (!Talkable.isTalking && PlayerItemMenu.openDetailMenu == -1)//當玩家沒有正在對話或是打開菜單
        {
            if (h != 0 || v != 0)//按方向鍵
            {
                doMove(h, v);
            }
            else if(submit > 0 && overRestTime)
            {
                doSubmit();
            }
            else if (cancel > 0 && isStatus("idle", currentState))//站穩才能打開菜單
            {
                doCancel();
            }
            else//什麼按也不按
            {
                shouldStatus = "idle";
            }
        }
        else if (PlayerItemMenu.openDetailMenu == 0)//打開一級菜單時的操作
        {
            if (cancel > 0 && overRestTime) doReturnCancel();
        }
    }
    #endregion

    #region 設定批次狀態機的狀態
    private void setStatus(string status)
    {
        this.animator.SetBool("idle", false);
        this.animator.SetBool("run", false);
        this.animator.SetBool("lookNote", false);
        if (!status.Equals("allClose"))//只開啟所設定的狀態，若設定allClose則狀態全關
        {
            this.animator.SetBool(status, true);
        }
    }
    #endregion

    #region 判斷當前狀態
    private bool isStatus(string status, AnimatorStateInfo nowStateHash)
    {
        bool b = false;
        switch (status)
        {
            case "idle":
                b = nowStateHash.IsName("Idle");
                break;
            case "run":
                b = nowStateHash.IsName("Base Layer.Run");
                break;
            case "lookNote":
                b = nowStateHash.IsName("Base Layer.TakeNote") || nowStateHash.IsName("Base Layer.LookNote");
                break;
            default:
                Debug.Log(status + "找不到相對應的狀態，可能輸入錯誤");
                break;
        }
        return b;
    }
    #endregion

    #region 將當前狀態設置為應該狀態
    private void doSetStatusToShould(AnimatorStateInfo nowStateHash)
    {
        string s = (isStatus(shouldStatus, nowStateHash) || isStatus("idle", nowStateHash)) ? shouldStatus : "idle";
        setStatus(s);
    }
    #endregion

    # region 角色移動
    private void doMove(float h, float v)
    {
        nowYaxis = this.transform.position.y;//取得當前位置
        float gapYaxis = nowYaxis - preYaxis;//取得上一幀與當前位置Y軸之差
        //角色移動
        rigidbody.AddForce(new Vector2(h * speedH, v * speedV));
        this.transform.Translate(0, 0, -(gapYaxis * speedZ), Space.World);

        //判斷方向，以便顯示正確的動畫
        moveDirction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirction.x = -1F;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDirction.x = 1F;
        if (Input.GetKey(KeyCode.UpArrow))
            moveDirction.y = 1F;
        if (Input.GetKey(KeyCode.DownArrow))
        moveDirction.y = -1F;
        shouldStatus = "run";
        preMoveDirction = moveDirction;
        animator.SetFloat("move_X", moveDirction.x);//賦值給Animator的相應變量
        animator.SetFloat("move_Y", moveDirction.y);

        //根據移動方向移動Player底下的talkColliderObject物件
        talkColliderObject.transform.position = new Vector2(this.transform.position.x + moveDirction.x * talkColliderPositionRateX, this.transform.position.y + moveDirction.y * talkColliderPositionRateY);

        preYaxis = nowYaxis;//更新上一幀的玩家位置信息，以便下一幀計算差值
    }
    #endregion

    # region 按下submit的操作
    private void doSubmit()
    {
        returnRestTimer();
        shouldStatus = "idle";
        talkColliderObject.SendMessage("doSurvey");
    }
    #endregion

    #region 在待機狀態，按下cancel的操作
    private void doCancel()
    {
        returnRestTimer();
        shouldStatus = "lookNote";
        PlayerItemMenu.openDetailMenu = 0;
        MenuObject.SendMessage("doOpenMenu");
    }
    #endregion

    #region 在打開菜單狀態，按下cancel的操作
    private void doReturnCancel()
    {
        returnRestTimer();
        shouldStatus = "idle";
        PlayerItemMenu.openDetailMenu = -1;
        MenuObject.SendMessage("doCloseMenu");
    }
    #endregion
}