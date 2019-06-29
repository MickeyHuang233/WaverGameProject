using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

//每一個場景初始化的操作，一定要每個場景都加，否則無法傳送到指定位置
public class InitSceneManagment : MonoBehaviour
{
    //要傳送的位置名
    public static string positionName;

    //玩家物件
    private GameObject playerObject;

    //傳送位置物件名稱，主要從Portal賦值
    public static string targetPositionName { get => positionName; set => positionName = value; }

    //指定傳送位置，主要讀檔用
    public static Vector3 targetPosition;

    [Header("視角移動速度")]
    [Range(0F, 50F)]
    public float moveSpeed;

    [Header("轉換場景後淡入等待秒數")]
    [Range(0.0F, 5.0F)]
    public float SceneOffTime = 0.5F;

    //場景是否需要淡出
    public static bool isNeedScenceDark = false;

    //目標場景
    public static string targetSceneName;

    [Header("需要場景初始化時調整Z軸的父物件(資料夾)")]
    public List<GameObject> modifyZAxisFaterObjects;

    [Header("顯示第一層黑的TileMap")]
    public Tilemap DarkMap_5;

    [Header("顯示第二層黑的TileMap")]
    public Tilemap DarkMap_4;

    [Header("顯示第三層黑的TileMap")]
    public Tilemap DarkMap_3;

    [Header("顯示第四層黑的TileMap")]
    public Tilemap DarkMap_2;

    [Header("顯示第五層黑的TileMap，暫時不需用")]
    public Tilemap DarkMap_1;

    [Header("顯示黑的TileMap要比照的區域")]
    public Tilemap BackgroundMap;

    [Header("地圖類型")]
    public string mapType;

    //Layout加載的類型
    public static string mapLayoutType;

    [Header("指定第一層黑的Tile")]
    public Tile DarkTile_5;

    [Header("指定第二層黑的Tile")]
    public Tile DarkTile_4;

    [Header("指定第三層黑的Tile")]
    public Tile DarkTile_3;

    [Header("指定第四層黑的Tile")]
    public Tile DarkTile_2;

    [Header("指定第五層黑的Tile，暫時不需用")]
    public Tile DarkTile_1;

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

        modifyZAxis();//調整Z軸位置
        mapLayoutType = mapType;
        showDarkMap();//顯示場景黑

        StartCoroutine(doChangeSceneOffInSingleScene());//協程_場景淡入
    }
    #endregion

    #region Update()
    void Update()
    {
        if (GameTimer.nactivesecond >= 0) showNagativeTime();
        loadTargetScene();//場景移動
    }
    #endregion

    #region 協程_場景淡入 doChangeSceneOffInSingleScene()
    IEnumerator doChangeSceneOffInSingleScene()
    {
        yield return new WaitForSeconds (SceneOffTime);
        //前置作業完成後淡入場景
        if (!CameraFix.sceneGradientIsStatus("SceneWhite") && isNeedScenceDark == true)
        {
            CameraFix.doChangeSceneOff();
            isNeedScenceDark = false;
        }
        showSceneName();//顯示場景名稱
    }
    #endregion

    #region 顯示場景黑   showDarkMap()
    private void showDarkMap()
    {
        //地圖為1F和3F只有兩個DarkLayout
        if (InitSceneManagment.mapLayoutType.Equals("1F") || InitSceneManagment.mapLayoutType.Equals("3F") || InitSceneManagment.mapLayoutType.Equals("B1"))
        {
            DarkMap_5.origin = DarkMap_4.origin = BackgroundMap.origin;
            DarkMap_5.size = DarkMap_4.size = BackgroundMap.size;
            foreach (Vector3Int p in DarkMap_5.cellBounds.allPositionsWithin) DarkMap_5.SetTile(p, DarkTile_5);
            foreach (Vector3Int p in DarkMap_4.cellBounds.allPositionsWithin) DarkMap_4.SetTile(p, DarkTile_4);
            if (InitSceneManagment.mapLayoutType.Equals("B1"))
            {
                DarkMap_3.origin = DarkMap_2.origin = BackgroundMap.origin;
                DarkMap_3.size = DarkMap_2.size = BackgroundMap.size;
                foreach (Vector3Int p in DarkMap_3.cellBounds.allPositionsWithin) DarkMap_3.SetTile(p, DarkTile_3);
                foreach (Vector3Int p in DarkMap_2.cellBounds.allPositionsWithin) DarkMap_2.SetTile(p, DarkTile_2);
            }
        }
    }
    #endregion

    #region 調整Z軸位置   modifyZAxis()
    private void modifyZAxis()
    {
        foreach (GameObject modifyZAxisFaterObject in modifyZAxisFaterObjects)
        {
            int faterObjectSize = modifyZAxisFaterObject.transform.childCount;
            for (int i = 0; i < faterObjectSize ; i++)
            {
                GameObject c = modifyZAxisFaterObject.transform.GetChild(i).gameObject;
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.y * PlayerMovement.multiplyZ);
            }
        }
        playerObject.transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.y * PlayerMovement.multiplyZ);
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
        if (!SceneManager.GetActiveScene().name.Equals(targetSceneName))
        {
            CameraFix.doChangeSceneOn();
            isNeedScenceDark = true;
            if (CameraFix.sceneGradientIsStatus("SceneDark")) SceneManager.LoadScene(targetSceneName);//淡出完成後才轉換場景
        }
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
