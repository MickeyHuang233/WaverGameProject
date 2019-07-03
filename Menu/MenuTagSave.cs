using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    菜單，物品欄畫面
    掛載物件：
        MenuTagSave --> SavePage
    實現功能：
        1. 控制動畫的開啟和關閉
        2. 檔案信息顯示及顯示當前劇情目標
        3. 存檔【未完成，存檔時顯示存檔中的動畫，存檔完成後播放動畫並關閉菜單】
        4. 返回上一層菜單
*/
public class MenuTagSave : MonoBehaviour
{
    //存檔菜單文字物件
    GameObject savePage;

    //指標物件
    private GameObject itemIndex;

    //顯示當前時間的物件
    private GameObject NowPlayTimerObject;
    //顯示當前地點的物件
    private GameObject NowPlayPositionObject;
    //顯示劇情目標的物件
    private GameObject PlotDescrptionObject;

    //顯示存檔信息物件
    private List<GameObject> saveInformations = new List<GameObject>();

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    //玩家物件
    private GameObject playerObject;

    //是否正在存檔
    private bool isSaving = false;

    void Start()
    {
        //取得savePage物件及動畫信息
        savePage = GameObject.Find("SavePage");
        //取得指標物件
        itemIndex = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = transform.GetChild(2).gameObject.transform.childCount - 1;
        //取得顯示當前時間的物件
        NowPlayTimerObject = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        //取得顯示當前地點的物件
        NowPlayPositionObject = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        //取得顯示劇情目標的物件
        PlotDescrptionObject = transform.GetChild(0).gameObject;
        //初始化指標物件的位置
        Vector3 choicePosition = transform.GetChild(2).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.2F, choicePosition.y, choicePosition.z);
        //取得顯示存檔信息物件
        for (int i = 1; i <= tagIndexMax; i++) saveInformations.Add(transform.GetChild(2).gameObject.transform.GetChild(i).gameObject);
        //初始化指標位置
        initializeIndexPosition();
        //取得玩家物件
        playerObject = GameObject.Find("Player");
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        if (PlayerItemMenu.openDetailMenu == 2 && !isSaving)
        {
            NowPlayTimerObject.GetComponent<Text>().text = showNowGameTimer(GameTimer.second, GameTimer.minute);
            int mapIndex = GameMenager.IsInMapDefinition(InitSceneManagment.targetSceneName);
            NowPlayPositionObject.GetComponent<Text>().text = showPositionById(mapIndex);
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetKeyDown(KeyCode.Z) && PlayerItemMenu.overRestTime) doSubmit();
        }

    }

    #region 初始化指標位置 initializeIndexPosition()
    private void initializeIndexPosition()
    {
        //存檔指標指向最近存的檔案
        long max = GameMenager.gameFiles.gameFiles[1].saveTime;
        tagIndex = 1;
        for (int i = 2; i <= tagIndexMax; i++)
        {
            long saveTime = GameMenager.gameFiles.gameFiles[i].saveTime;
            if (saveTime > max)
            {
                max = saveTime;
                tagIndex = i;
            }
        }
        //更新指標位置
        doMove(0);
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
        else if(minute >= 10 && minute < 100) minuteStr = "0" + minute.ToString();
        else if (minute >= 100 && minute < 1000) minuteStr =  minute.ToString();
        else minuteStr = "999";
        return minuteStr + ":" + secondStr;
    }
    #endregion

    #region 顯示當前地點    showPositionById(int mapIndex)
    private string showPositionById(int mapIndex)
    {
        bool shouldShowSceneName = GameMenager.mapInformationList[mapIndex].IsShowMapName;
        string sceneName = (mapIndex==0)? "-------" : (shouldShowSceneName) ? GameMenager.mapInformationList[mapIndex].MapName : "？？？";
        return sceneName;
    }
    #endregion

    #region 顯示當前劇情目標    showNowPosition()
    private void showSituactionTarget()
    {
        Plot situactionTargetPlot = GameMenager.findPlotById(GameMenager.gamePlotNumber);
        PlotDescrptionObject.GetComponent<Text>().text = situactionTargetPlot.SituactionTarget;
    }
    #endregion

    #region 顯示存檔信息    showSaveInformation()
    private void showSaveInformation()
    {
        //顯示存檔信息
        for (int i = 0; i < saveInformations.Count; i++)
        {
            //存檔時玩家所在的地圖名稱
            int mapIndex = GameMenager.gameFiles.gameFiles[i+1].mapId;
            saveInformations[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = showPositionById(mapIndex);
            //存檔時的已遊玩時間
            int second = GameMenager.gameFiles.gameFiles[i + 1].gameTimeSecond;
            int minute = GameMenager.gameFiles.gameFiles[i + 1].gameTimeMinute;
            saveInformations[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = showNowGameTimer(second, minute);
        }
    }
    #endregion

    #region 打開存檔菜單  showPage()
    private void showPage()
    {
        showSaveInformation();//顯示存檔信息
        showSituactionTarget();//顯示當前劇情目標
    }
    #endregion

    #region 關閉存檔菜單
    private void hidePage()
    {

    }
    #endregion

    #region 指標移動    doMove(float v)
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//第一項，向上移動
        else if (v > 0 && tagIndex == 1) tagIndex = tagIndexMax;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
        else if (v < 0 && tagIndex == tagIndexMax) tagIndex = 1;//最後一項，向下移動
        //更新指標位置
        Vector3 choicePosition = this.transform.GetChild(2).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.2F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 按下確認鍵操作 doSubmit()
    private void doSubmit()
    {
        StartCoroutine(doShowPlayerBubble());
        GameMenager.saveToJsonFile(tagIndex);//遊戲當前信息轉為json並存到硬中
        PlayerItemMenu.returnRestTimer();
    }
    #endregion

    #region 協程
    IEnumerator doShowPlayerBubble()
    {
        isSaving = true;
        playerObject.SendMessage("showTalkBubble_01");
        saveInformations[tagIndex - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = "覆寫中……";
        saveInformations[tagIndex - 1].transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(3F);
        showSaveInformation();//刷新存檔信息
        playerObject.SendMessage("hideTalkBubble");
        transform.parent.parent.SendMessage("doCloseDetailMenu");
        isSaving = false;
    }
    #endregion
}
