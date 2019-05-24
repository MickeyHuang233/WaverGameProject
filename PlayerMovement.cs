using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制主角移動及傳送
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
    public float restTime = 0.1F;

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

    //菜單物件
    private GameObject MenuObject;

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
    
    void Update()
    {
        currentState = this.animator.GetCurrentAnimatorStateInfo(0);//取得當前動畫狀態的hashCode
        float h = Input.GetAxisRaw("Horizontal");//檢測水平移動
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        float cancel = Input.GetAxisRaw("Cancel");//檢測x鍵
        doSetStatusToShould(currentState);
        if (!Talkable.isTalking && !isStatus("lookNote", currentState))//當玩家沒有正在對話或是打開菜單
        {
            restTimer += Time.deltaTime;
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
        else if (isStatus("lookNote", currentState) && !PlayerItemMenu.openDetailMenu)//打開一級菜單時的操作
        {
            restTimer += Time.deltaTime;
            if (cancel > 0 && overRestTime)
            {
                doReturnCancel();
            }
        }
    }

    //設定批次狀態機的狀態
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

    //判斷當前狀態
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

    //將當前狀態設置為應該狀態
    private void doSetStatusToShould(AnimatorStateInfo nowStateHash)
    {
        string s = (isStatus(shouldStatus, nowStateHash) || isStatus("idle", nowStateHash)) ? shouldStatus : "idle";
        setStatus(s);
    }

    //角色移動
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

    //按下submit的操作
    private void doSubmit()
    {
        returnRestTimer();
        shouldStatus = "idle";
        talkColliderObject.SendMessage("doSurvey");
    }

    //在待機狀態，按下cancel的操作
    private void doCancel()
    {
        returnRestTimer();
        shouldStatus = "lookNote";
        MenuObject.SendMessage("doOpenMenu");
    }

    //在打開菜單狀態，按下cancel的操作
    private void doReturnCancel()
    {
        returnRestTimer();
        shouldStatus = "idle";
        MenuObject.SendMessage("doCloseMenu");
    }

    //已休息時間歸零
    private void returnRestTimer()
    {
        restTimer = 0F;
    }

}
