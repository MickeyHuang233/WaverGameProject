using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTagSave : MonoBehaviour
{
    //存檔菜單文字物件
    GameObject savePage;

    //存檔菜單文字的動畫信息
    private Animator savePageAanimator;

    //Menu物件
    GameObject MenuObject;

    //指標物件
    GameObject itemIndex;

    //顯示當前時間的物件
    GameObject NowPlayTimer;
    //顯示當前地點的物件
    GameObject NowPlayPosition;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    void Start()
    {
        //取得savePage物件及動畫信息
        savePage = GameObject.Find("SavePage");
        savePageAanimator = savePage.GetComponent<Animator>();
        //取得Menu物件
        MenuObject = this.transform.parent.parent.gameObject;
        //取得指標物件
        itemIndex = this.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = this.transform.GetChild(2).gameObject.transform.childCount - 1;
        //取得顯示當前時間的物件
        NowPlayTimer = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        //取得顯示當前地點的物件
        NowPlayPosition = this.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        //初始化指標物件的位置
        Vector3 choicePosition = this.transform.GetChild(2).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.2F, choicePosition.y, choicePosition.z);
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        if (PlayerItemMenu.openDetailMenu == 2)
        {
            showNowGameTimer();
            showNowPosition();
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetKeyDown(KeyCode.Z) && PlayerItemMenu.overRestTime) doSubmit();
        }

    }

    #region 顯示當前遊戲時間    showNowGameTimer()
    private void showNowGameTimer()
    {
        string secondStr;
        if (GameTimer.second < 10) secondStr = "0" + GameTimer.second.ToString();
        else secondStr = GameTimer.second.ToString();
        string minuteStr = GameTimer.minute.ToString();
        if(GameTimer.minute < 10) minuteStr = "00" + GameTimer.minute.ToString();
        else if(GameTimer.minute >= 10 && GameTimer.minute < 100) minuteStr = "0" + GameTimer.minute.ToString();
        else if (GameTimer.minute >= 100 && GameTimer.minute < 1000) minuteStr =  GameTimer.minute.ToString();
        else minuteStr = "999";
        NowPlayTimer.GetComponent<Text>().text = minuteStr + ":" + secondStr;
    }
    #endregion

    #region 顯示當前地點    showNowPosition()
    private void showNowPosition()
    {
        int mapIndex = GameMenager.IsInMapDefinition(InitSceneManagment.targetSceneName);
        bool shouldShowSceneName = GameMenager.mapInformationList[mapIndex].IsShowMapName;
        string sceneName = (shouldShowSceneName) ? GameMenager.mapInformationList[mapIndex].MapName : "？？？";
        NowPlayPosition.GetComponent<Text>().text = sceneName;
    }
    #endregion

    #region 打開存檔菜單  showSavePage()
    private void showSavePage()
    {
        savePageAanimator.SetBool("openDetilMenu", true);
        savePageAanimator.SetBool("closeDetilMenu", false);
    }
    #endregion

    #region 關閉存檔菜單      hideSavePage()
    private void hideSavePage()
    {
        savePageAanimator.SetBool("openDetilMenu", false);
        savePageAanimator.SetBool("closeDetilMenu", true);
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
        Debug.Log("Save_doSubmit()");
        PlayerItemMenu.returnRestTimer();
    }
    #endregion
}
