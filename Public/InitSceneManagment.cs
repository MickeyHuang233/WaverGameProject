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

    //用來管理DarkLayout的物件
    private Transform darkLayoutHolder;

    [Header("地圖類型")]
    public string mapType;

    [Header("1F顯示物件")]
    public GameObject[] darkLayout_1F;

    //傳送位置物件名稱，主要從Portal賦值
    public static string targetPositionName { get => positionName; set => positionName = value; }

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

        getTargetPositionObject(); //取得指定傳送點物件

        createDarkLayout();//建立前景黑的物件
        darkLayoutToList();//將DarkLayout的全部子物件放入List中

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
        darkLayoutMove();//DarkLayout的移動
        loadTargetScene();//場景移動
    }
    #endregion

    #region 取得指定傳送點物件    getTargetPositionObject()
    private void getTargetPositionObject()
    {
        GameObject targetPositionObject = GameObject.Find(positionName) as GameObject;
        if (targetPositionObject != null) playerObject.transform.position = targetPositionObject.transform.position;
        else Debug.Log(positionName + " is not found!!");
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

    #region 建立前景黑的物件    createDarkLayout()
    private void createDarkLayout()
    {
        darkLayoutHolder = new GameObject("DarkLayout").transform;//建立一個新的物件叫Map，並取得這個物件的transform屬性
        if (mapType.Equals("1F"))//依地圖類型分産出的DarkLayout物件，默認是什麼都不産出
        {
            for (int i = 0; i < darkLayout_1F.Length; i++)
            {
                GameObject go = GameObject.Instantiate(darkLayout_1F[i], new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, 0), Quaternion.identity) as GameObject;
                go.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.5F);
                go.transform.SetParent(darkLayoutHolder);//將新建出來的地圖放到darkLayoutHolder物件底下
            }
        }
        else if (mapType.Equals("3F"))
        {
            //TODO 需加3F的DarkLayout産生
        }
        else if (mapType.Equals("B1"))
        {
            //TODO 需加B1的DarkLayout産生
        }
    }
    #endregion

    #region 將DarkLayout的全部子物件放入List中    darkLayoutToList()
    private void darkLayoutToList()
    {
        darkLayoutObject = GameObject.Find("DarkLayout");
        for (int i = 0; i < darkLayoutObject.transform.GetChildCount(); i++)
        {
            GameObject d = darkLayoutObject.transform.GetChild(i).gameObject;
            darkLayoutObjects.Add(d);
        }
    }
    #endregion

    #region DarkLayout的移動   darkLayoutMove()
    private void darkLayoutMove()
    {
        float v = (PlayerMovement.moveDirction.x > 0) ? 0.5F : (PlayerMovement.moveDirction.x == 0) ? 0F : -0.5F;
        float h = (PlayerMovement.moveDirction.y > 0) ? 0.5F : (PlayerMovement.moveDirction.y == 0) ? 0F : -0.5F;
        //取得玩家位置
        float playerX = playerObject.transform.position.x + v;
        float playerY = playerObject.transform.position.y + h;

        for (int i = 0; i < darkLayoutObjects.Count; i++)
        {
            //TODO 使用蒙版實現場景渲染
            GameObject d = darkLayoutObjects[i];
            darkLayoutTargetPosition = new Vector3(playerX, playerY, 0);
            d.transform.position = Vector3.Lerp(d.transform.position, darkLayoutTargetPosition, moveSpeed * Time.deltaTime / (i+1));
        }
    }
    #endregion

    #region 倒數秒數加10秒
    public void addNagativeTiming10Second()
    {
        GameTimer.nactivesecond += 10;
    }
    #endregion
}
