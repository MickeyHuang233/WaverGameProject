using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

//每一個場景初始化的操作，一定要每個場景都加，否則無法傳送到指定位置
public class InitSceneManagment : MonoBehaviour
{
    //要傳送的位置名
    static string positionName;

    //玩家物件
    private GameObject playerObject;

    //傳送位置物件名稱，主要從Portal賦值
    public static string targetPositionName { get => positionName; set => positionName = value; }

    //指定傳送位置，主要讀檔用
    public static Vector3 targetPosition;

    //用來管理DarkLayout的物件(父類)
    private GameObject darkLayoutObject;

    //用來管理DarkLayout的物件(子類)
    private List<GameObject> darkLayoutObjects = new List<GameObject>();

    //目標位置
    private Vector3 darkLayoutTargetPosition;

    [Header("視角移動速度")]
    [Range(0F, 50F)]
    public float moveSpeed;

    //場景是否需要淡出
    public static bool isNeedScenceDark = false;

    //目標場景
    public static string targetSceneName;

    //場景轉換是否已顯示過場景名稱
    private bool showedSceneName;

    #region Start()
    void Start()
    {
        //取得玩家物件
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.Log("Tag 'Player' is not found!!");
            return;//找不到玩家物件直接返回
        }
        PlayerMovement.returnRestTimer();//玩家已休息時間歸零

        getTargetPosition(); //取得指定傳送點物件

        //加載GameManager
        Talkable.flowchartManager = GameObject.Find("SpeakManager").GetComponent<Flowchart>();

        //初始化目標場景為當前場景
        targetSceneName = SceneManager.GetActiveScene().name;

        //前置作業完成後淡入場景
        if (!CameraFix.sceneGradientIsStatus("SceneWhite") && isNeedScenceDark == true)
        {
            GameObject.Find("Main Camera").SendMessage("doChangeSceneOff");
            isNeedScenceDark = false;
        }

        showedSceneName = false;//剛進入場景的話就沒有顯示場景名稱
    }
    #endregion

    #region Update()
    void Update()
    {
        if (!showedSceneName) showSceneName();//顯示場景名稱
        if (GameTimer.nactivesecond >= 0) showNagativeTime();
        loadTargetScene();//場景移動
    }
    #endregion

    #region 取得指定傳送點物件    getTargetPositionObject()
    private void getTargetPosition()
    {
        GameObject targetPositionObject = GameObject.Find(positionName) as GameObject;
        if (targetPositionObject != null)//當指定目標物件
        {
            playerObject.transform.position = targetPositionObject.transform.position;
            targetPositionObject = null;
        }
        else
        {
            if (targetPosition.x!=-9999F && targetPosition.y!=-9999F && targetPosition.z != -9999F)//當指定作坐標
            {
                playerObject.transform.position = InitSceneManagment.targetPosition;
                InitSceneManagment.targetPosition = new Vector3(-9999F, -9999F, -9999F);
            }
            else
            {
                Debug.Log("targetPositionObject == null && targetPosition == null");
            }
        }
    }
    #endregion

    #region 場景移動的實際操作   loadTargetScene()
    private void loadTargetScene()
    {
        //如果目標場景為空或為空字串就報錯並返回
        if (targetSceneName == null || targetSceneName.Equals(""))
        {
            Debug.Log("目標場景名稱(targetScenceName)為空或為空字串，故無法轉移場景");
            return;
        }
        //如果當前場景和目標場景不一致時，加載目標場景
        if (!SceneManager.GetActiveScene().name.Equals(targetSceneName) && CameraFix.sceneGradientIsStatus("SceneDark"))
            SceneManager.LoadScene(targetSceneName);
    }
    #endregion

    #region 顯示場景名稱   showSceneName()
    private void showSceneName()
    {
        if(GameMenager.IsInMapDefinition(targetSceneName) != -1)//如果Unity場景名稱可以在定義檔找到才顯示場景名
        {
            bool shouldShowSceneName = GameMenager.mapInformationList[GameMenager.IsInMapDefinition(targetSceneName)].IsShowMapName;
            string sceneName = (shouldShowSceneName) ? GameMenager.mapInformationList[GameMenager.IsInMapDefinition(targetSceneName)].MapName : "？？？";
            CameraFix.showSceneName(sceneName);
        }
        AnimatorStateInfo currentState = GameObject.Find("Canvas_UI").transform.GetChild(1).gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);//取得當前動畫狀態
        bool isSceneNamehide = currentState.IsName("HideSceneName");
        if(isSceneNamehide) showedSceneName = true;
    }
    #endregion

    #region 顯示倒數時間   showNagativeTime()
    /*
    若需要倒數直接指定GameTimer.nactivesecond要倒數的秒數即可 
    */
    private void showNagativeTime()
    {
        string timer =
            (GameTimer.nactivesecond >= 100) ? GameTimer.nactivesecond.ToString() + "秒" :
            (GameTimer.nactivesecond >= 10) ? "0" + GameTimer.nactivesecond.ToString() + "秒" : 
            "00" + GameTimer.nactivesecond.ToString() + "秒";
        CameraFix.showNagativeTime(timer);
    }
    #endregion

    #region 倒數秒數加10秒
    public void addNagativeTiming10Second()
    {
        GameTimer.nactivesecond += 10;
    }
    #endregion
}
