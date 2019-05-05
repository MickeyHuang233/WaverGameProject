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

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowYaxis = this.transform.position.y;
        preYaxis = this.transform.position.y;
        //取得玩家物件下的對話域物件的碰撞器元件
        talkColliderObject = this.transform.GetChild(0).gameObject;
    }
    
    void Update()
    {
        if (!Talkable.isTalking)//沒在對話才可行走
        {
            doMove();
        }
        else//如果正在對話就將狀態改為待機
        {
            setStatus("idle");
        }
    }

    //設定批次狀態機的狀態
    private void setStatus(string status)
    {
        this.animator.SetBool("idle", false);
        this.animator.SetBool("run", false);
        if (!status.Equals("allClose"))//只開啟所設定的狀態，若設定allClose則狀態全關
        {
            this.animator.SetBool(status, true);
        }
    }

    //角色移動
    private void doMove()
    {
        float h = Input.GetAxisRaw("Horizontal");//檢測水平移動
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        nowYaxis = this.transform.position.y;//取得當前位置
        float gapYaxis = nowYaxis - preYaxis;//取得上一幀與當前位置Y軸之差
        //角色移動
        rigidbody.AddForce(new Vector2(h * speedH, v * speedV));
        this.transform.Translate( 0, 0, -(gapYaxis * speedZ), Space.World);

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
        if (moveDirction.sqrMagnitude > 0)//當玩家有按上下左右任一鍵時
        {
            setStatus("run");
            preMoveDirction = moveDirction;
        }
        else//當玩家沒執行任何操作就把狀態改為待機
        {
            setStatus("idle");
            moveDirction = preMoveDirction;
        }
        animator.SetFloat("move_X", moveDirction.x);//賦值給Animator的相應變量
        animator.SetFloat("move_Y", moveDirction.y);

        //根據移動方向移動Player底下的talkColliderObject物件
        talkColliderObject.transform.position = new Vector2(this.transform.position.x + moveDirction.x * talkColliderPositionRateX, this.transform.position.y + moveDirction.y * talkColliderPositionRateY);
        //doMoveV(v);
        //doMoveH(h);

        preYaxis = nowYaxis;//更新上一幀的玩家位置信息，以便下一幀計算差值
    }

    //上下鍵的判斷
    private void doMoveV(float v)
    {
        Debug.Log("doMoveV");
        if (v > 0)//上走
        {
        }
        else if (v < 0)//下走
        {
        }
        else//h == 0
        {
        }
    }

    //左右移動的判斷
    private void doMoveH(float h)
    {
        Debug.Log("doMoveH");
        if (h > 0)//右走
        {
        }
        else if (h < 0)//左走
        {
        }
    }
}
