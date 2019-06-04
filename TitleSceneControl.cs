using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    遊戲初始畫面的Button方法
    掛載物件：
        TitleSceneControl --> 
    實現功能：
        1. 遊戲開始後加載初始化場景 // TODO 畫面需要淡出
        2. // TODO 遊戲讀取的操作
        3. // TODO 遊戲關閉的操作
*/
public class TitleSceneControl : MonoBehaviour
{
    //初始化場景名
    public string startScene;

    //玩家所選的操作
    private string selectButton = "Null";

    //相機物件
    private GameObject cameraObject;

    #region Start()
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
    }
    #endregion

    #region Update()
    void Update()
    {
        if (Input.GetButtonDown("Submit")) doGameStart();//暫時使用
        if (selectButton.Equals("Start"))
        {
            if(CameraFix.isStatus("SceneDark")) SceneManager.LoadScene(startScene);//新的加找場景方法
            else cameraObject.SendMessage("doChangeSceneOn");
        }
        else if (selectButton.Equals("Load"))
        {

        }
        else if (selectButton.Equals("Exit"))
        {
            if (CameraFix.isStatus("SceneDark")) Application.Quit();//關閉遊戲
            else cameraObject.SendMessage("doChangeSceneOn");
        }
    }
    #endregion

    #region 遊戲開始
    public void doGameStart()
    {
        selectButton = "Start";
    }
    #endregion

    #region 讀取遊戲
    public void doGameLoad()
    {
        Debug.Log("doGameLoad");
        selectButton = "Load";
    }
    #endregion

    #region 關閉遊戲
    public void doGameExit()
    {
        Debug.Log("doGameExit");
        selectButton = "Exit";
    }
    #endregion
}
