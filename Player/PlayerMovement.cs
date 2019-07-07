using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    控制主角移動及傳送
    掛載物件：
        PlayerMovement --> Player(命名固定)
        SurveyJudge --> (碰撞器物件必須放在玩家物件底下的第一個)
    實現功能：
        1. 角色的拸動及動畫轉換// TODO 缺斜向四個角的動畫轉換
        2. 在角色上下移動時需要同時改動Z軸
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

    //抏家物件
    public static GameObject playerObject;

    //玩家應該的狀態
    private string shouldStatus;

    //當前角色方向
    public static Vector2 moveDirction;

    //對話域物件
    private GameObject talkColliderObject;

    [Header("角色跑步速度_縱向")]
    [Range(0.5F, 20F)]
    public float speedRunV = 10F;

    [Header("角色跑步速度_橫向")]
    [Range(0.5F, 20F)]
    public float speedRunH = 8F;

    [Header("角色步行速度_縱向")]
    [Range(0.5F, 20F)]
    public float speedWalkV = 5F;

    [Header("角色步行速度_橫向")]
    [Range(0.5F, 20F)]
    public float speedWalkH = 4F;

    [Header("角色步行速度_Z")]
    [Range(-1F, 1F)]
    public float playerMultiplyZ = 0.05F;

    //得出Z軸乘率：Z軸位置 = Y軸位置 * multiplyZ
    //值由playerMultiplyZ設定
    public static float multiplyZ;

    [Header("調查用的物件位置所乘倍率_X")]
    [Range(-1F, 1F)]
    public float talkColliderPositionRateX = 0.1F;

    [Header("調查用的物件位置所乘倍率_Y")]
    [Range(-1F, 1F)]
    public float talkColliderPositionRateY = 0.1F;

    [Header("對話完成或關閉菜單所需等待時間")]
    [Range(0F, 5F)]
    public float setRestTime = 0.5F;

    //菜單物件
    private GameObject MenuObject;

    //對話完成或關閉菜單應休息時間
    public static float restTime;

    //計算玩家已休息時間
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
        playerObject = gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MenuObject = transform.GetChild(3).gameObject;
        talkColliderObject = transform.GetChild(2).gameObject;//取得玩家物件下的對話域物件的碰撞器元件
        //將設定的角色步行速度_Z放至得出Z軸乘率，以方便調用
        multiplyZ = playerMultiplyZ;
        //將設定的應休息時間放至靜態變量，以方便調用
        restTime = setRestTime;
        shouldStatus = "idle";
        //根據玩家Y軸計算Z軸的值
        playerObject.transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.y * PlayerMovement.multiplyZ);
    }
    #endregion

    #region Update()
    void Update()
    {
        Debug.Log(PlayerItemMenu.openDetailMenu);
        currentState = animator.GetCurrentAnimatorStateInfo(0);//取得當前動畫狀態的hashCode
        float h = Input.GetAxisRaw("Horizontal");//檢測水平移動
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float shift = Input.GetAxisRaw("Shift");//檢測點擊左Shift鍵
        doSetStatusToShould(currentState);
        restTimer += Time.deltaTime;
        if (!Talkable.isTalking && PlayerItemMenu.openDetailMenu == -1)//當玩家沒有正在對話或是打開菜單
        {
            if (h != 0 || v != 0) doMove(h, v, shift);//按方向鍵
            else if(Input.GetButtonDown("Submit") && overRestTime) doSubmit();
            else if (Input.GetButtonDown("Cancel") && isStatus("idle", currentState) && overRestTime) doCancel();//站穩才能打開菜單
            else shouldStatus = "idle";//什麼按也不按
        }
        //依據玩家Y軸計算出Z軸的值
        playerObject.transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.y * PlayerMovement.multiplyZ);
    }
    #endregion

    #region 設定批次狀態機的狀態
    private void setStatus(string status)
    {
        animator.SetBool("idle", false);
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("lookNote", false);
        if (!status.Equals("allClose"))//只開啟所設定的狀態，若設定allClose則狀態全關
        {
            animator.SetBool(status, true);
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
                b = nowStateHash.IsName("Base Layer.Idle");
                break;
            case "walk":
                b = nowStateHash.IsName("Base Layer.Walk");
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
    private void doMove(float h, float v, float shift)
    {
        //角色移動
        float moveSpeedH = (shift > 0)? speedRunH : speedWalkH;
        float moveSpeedV = (shift > 0) ? speedRunV : speedWalkV;
        rigidbody.AddForce(new Vector2(h * moveSpeedH, v * moveSpeedV));

        //判斷方向，以便顯示正確的動畫
        moveDirction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) moveDirction.x = -1F;
        if (Input.GetKey(KeyCode.RightArrow)) moveDirction.x = 1F;
        if (Input.GetKey(KeyCode.UpArrow)) moveDirction.y = 1F;
        if (Input.GetKey(KeyCode.DownArrow)) moveDirction.y = -1F;
        shouldStatus = (shift>0)? "run" : "walk";
        animator.SetFloat("move_X", moveDirction.x);//賦值給Animator的相應變量
        animator.SetFloat("move_Y", moveDirction.y);

        //根據移動方向移動Player底下的talkColliderObject物件
        talkColliderObject.transform.position = new Vector2(transform.position.x + moveDirction.x * talkColliderPositionRateX, transform.position.y + moveDirction.y * talkColliderPositionRateY);
    }
    #endregion

    # region 按下submit的操作
    private void doSubmit()
    {
        shouldStatus = "idle";
        talkColliderObject.SendMessage("doSurvey");
        returnRestTimer();
    }
    #endregion

    #region 在待機狀態，按下cancel的操作
    private void doCancel()
    {
        shouldStatus = "lookNote";
        MenuObject.SendMessage("doOpenMenu");
        SendMessage("hideTalkBubble");//清空玩家頭上的對話泡泡狀態
        returnRestTimer();
    }
    #endregion
}
