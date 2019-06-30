using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    遊戲初始畫面的Button方法
    掛載物件：
        TitleSceneControl --> TitleScenesManager
    實現功能：
        1. 遊戲開始後加載初始化場景
        2. 遊戲讀取的操作
        3. 遊戲關閉的操作
*/
public class TitleSceneControl : MonoBehaviour
{
    //初始化場景名
    public string startScene;

    //玩家所選的操作
    public static string selectButton = "Null";

    //主頁面按鍵物件
    private List<GameObject> menuButtonObject = new List<GameObject>();

    //第一層菜單指標物件
    private GameObject firstMenuIndex;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    //存檔欄當前位置編號
    public static int loadIndex = 1;

    //存檔欄最大位置編號
    private int loadIndexMax;

    //讀檔指標物件
    private GameObject SaveIndex;

    //顯示存檔信息物件
    private List<GameObject> SaveSituations = new List<GameObject>();


    #region Start()
    void Start()
    {
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = GameObject.Find("Canvas_TitleButton").transform.childCount - 1;
        //獲取主頁面按鍵物件
        for (int i=1; i<=tagIndexMax; i++) menuButtonObject.Add(GameObject.Find("Canvas_TitleButton").transform.GetChild(i).gameObject);

        //隱藏讀檔信息
        GameObject savePage = GameObject.Find("SavePage");
        savePage.GetComponent<Animator>().SetBool("openDetilMenu", false);
        savePage.GetComponent<Animator>().SetBool("closeDetilMenu", true);
        //獲取讀檔指標物件
        SaveIndex = savePage.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        //初始化存檔欄最大位置編號
        loadIndexMax = savePage.transform.GetChild(0).gameObject.transform.childCount - 1;
        //取得顯示存檔信息物件
        for (int i = 1; i <= loadIndexMax; i++) SaveSituations.Add(GameObject.Find("SaveSituations").transform.GetChild(i).gameObject);
        showSaveInformation();//顯示存檔信息

        //獲取第一層菜單指標物件
        firstMenuIndex = GameObject.Find("Canvas_TitleButton").transform.GetChild(0).gameObject;

        initializeIndexPosition();//初始化指標位置

        //初始化完成後才淡入場景
        GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Animator>().SetBool("changeScene_On", false);
        GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Animator>().SetBool("changeScene_Off", true);
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        if (selectButton.Equals("LoadPage"))//當打開二級菜單
        {
            if (Input.GetButtonDown("Vertical")) doCanceMove(v);
            if (Input.GetButtonDown("Cancel")) doCancelLoad();
            if (Input.GetButtonDown("Submit")) doSubmitLoad();
        }
        else
        {
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetButtonDown("Submit"))
            {
                if (tagIndex == 1) selectButton = "Start";//Start
                else if (tagIndex == 2) doGameLoad();//Load
                else if (tagIndex == 3) selectButton = "Exit";//Exit
            }
        }
        //根據selectButton的關鍵字決定要做什麼操作
        if (selectButton.Equals("Start"))
        {
            if (CameraFix.sceneGradientIsStatus("SceneDark")) SceneManager.LoadScene(startScene);//新的加找場景方法
            else CameraFix.doChangeSceneOn();
        }
        else if (selectButton.Equals("Load"))
        {
            if (CameraFix.sceneGradientIsStatus("SceneDark")) SceneManager.LoadScene(startScene);//新的加找場景方法
            else CameraFix.doChangeSceneOn();
        }
        else if (selectButton.Equals("Exit"))
        {
            if (CameraFix.sceneGradientIsStatus("SceneDark")) Application.Quit();//關閉遊戲
            else CameraFix.doChangeSceneOn();
        }
    }
    #endregion

    #region 初始化指標位置 initializeIndexPosition()
    private void initializeIndexPosition()
    {
        //初始化一級菜單指標物件的位置
        foreach (GameFile gameFile in GameMenager.gameFiles.gameFiles)
        {
            if(gameFile.mapId != 0)
            {
                tagIndex = 2;
                break;
            }
        }
        Vector3 choicePosition = menuButtonObject[tagIndex - 1].transform.position;
        firstMenuIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);

        //存檔指標指向最近存的檔案
        long max = GameMenager.gameFiles.gameFiles[1].saveTime;
        TitleSceneControl.loadIndex = 1;
        for (int i = 2; i <= loadIndexMax; i++)
        {
            long saveTime = GameMenager.gameFiles.gameFiles[i].saveTime;
            if (saveTime > max)
            {
                max = saveTime;
                TitleSceneControl.loadIndex = i;
            }
        }
        //更新指標位置
        doCanceMove(0);
    }
    #endregion

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//第一項，向上移動
        else if (v > 0 && tagIndex == 1) tagIndex = tagIndexMax;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
        else if (v < 0 && tagIndex == tagIndexMax) tagIndex = 1;//最後一項，向下移動
        //更新指標位置
        Vector3 choicePosition = menuButtonObject[tagIndex - 1].transform.position;
        firstMenuIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 顯示存檔信息    showSaveInformation()
    private void showSaveInformation()
    {
        for (int i = 0; i < SaveSituations.Count; i++)
        {
            //存檔時玩家所在的地圖名稱
            int mapIndex = GameMenager.gameFiles.gameFiles[i + 1].mapId;
            SaveSituations[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = showPositionById(mapIndex);
            //存檔時的已遊玩時間
            int second = GameMenager.gameFiles.gameFiles[i + 1].gameTimeSecond;
            int minute = GameMenager.gameFiles.gameFiles[i + 1].gameTimeMinute;
            SaveSituations[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = showNowGameTimer(second, minute);
        }
    }
    #endregion

    #region 顯示當前地點    showPositionById(int mapIndex)
    private string showPositionById(int mapIndex)
    {
        bool shouldShowSceneName = GameMenager.mapInformationList[mapIndex].IsShowMapName;
        string sceneName = (mapIndex == 0) ? "-------" : (shouldShowSceneName) ? GameMenager.mapInformationList[mapIndex].MapName : "？？？";
        return sceneName;
    }
    #endregion

    #region 顯示當前遊戲時間    showNowGameTimer()
    private string showNowGameTimer(int second, int minute)
    {
        string secondStr;
        //second format
        if (second < 10) secondStr = "0" + second.ToString();
        else secondStr = second.ToString();
        string minuteStr = minute.ToString();
        //minute format
        if (minute < 10) minuteStr = "00" + minute.ToString();
        else if (minute >= 10 && minute < 100) minuteStr = "0" + minute.ToString();
        else if (minute >= 100 && minute < 1000) minuteStr = minute.ToString();
        else minuteStr = "999";
        return minuteStr + ":" + secondStr;
    }
    #endregion

    #region 一級菜單，開啟存檔頁面 doGameLoad()
    public void doGameLoad()
    {
        selectButton = "LoadPage";
        GameObject.Find("SavePage").GetComponent<Animator>().SetBool("openDetilMenu", true);
        GameObject.Find("SavePage").GetComponent<Animator>().SetBool("closeDetilMenu", false);
    }
    #endregion

    #region 二級菜單，指標移動   doCanceMove(float v)
    public void doCanceMove(float v)
    {
        if (v > 0 && TitleSceneControl.loadIndex > 1) TitleSceneControl.loadIndex--;//第一項，向上移動
        else if (v > 0 && TitleSceneControl.loadIndex == 1) TitleSceneControl.loadIndex = loadIndexMax;//向上移動
        else if (v < 0 && TitleSceneControl.loadIndex < loadIndexMax) TitleSceneControl.loadIndex++;//向下移動
        else if (v < 0 && TitleSceneControl.loadIndex == loadIndexMax) TitleSceneControl.loadIndex = 1;//最後一項，向下移動
        //更新指標位置
        Vector3 choicePosition = SaveSituations[TitleSceneControl.loadIndex - 1].transform.position;
        SaveIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 返回，關閉讀取遊戲菜單 doCancelLoad()
    public void doCancelLoad()
    {
        selectButton = "Null";
        GameObject.Find("SavePage").GetComponent<Animator>().SetBool("openDetilMenu", false);
        GameObject.Find("SavePage").GetComponent<Animator>().SetBool("closeDetilMenu", true);
    }
    #endregion

    #region 返回，關閉讀取遊戲菜單 doSubmitLoad()
    public void doSubmitLoad()
    {
        if (GameMenager.gameFiles.gameFiles[TitleSceneControl.loadIndex].mapId != 0) selectButton = "Load";
        else Debug.Log("不可邊空檔案");
    }
    #endregion
}
