using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    遊戲初始畫面的Button方法
    掛載物件：
        TitleSceneControl --> TitleScenesManager
    實現功能：
        1. 遊戲開始後加載初始化場景
        2. // TODO 遊戲讀取的操作
        3. 遊戲關閉的操作
*/
public class TitleSceneControl : MonoBehaviour
{
    //初始化場景名
    public string startScene;

    //玩家所選的操作
    private string selectButton = "Null";

    //相機物件
    private GameObject cameraObject;

    //主頁面按鍵物件
    private List<GameObject> menuButtonObject = new List<GameObject>();

    //指標物件
    private GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    #region Start()
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = GameObject.Find("Canvas_TitleButton").transform.childCount - 1;
        //獲取主頁面按鍵物件
        for (int i=1; i<=tagIndexMax; i++) menuButtonObject.Add(GameObject.Find("Canvas_TitleButton").transform.GetChild(i).gameObject);
        //獲取指標物件
        itemIndex = GameObject.Find("Canvas_TitleButton").transform.GetChild(0).gameObject;
        //初始化指標物件的位置
        Vector3 choicePosition = menuButtonObject[tagIndex-1].transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        if (Input.GetButtonDown("Vertical")) doMove(v);
        if (Input.GetButtonDown("Submit")) doSubmit();
        //根據selectButton的關鍵字決定要做什麼操作
        if (selectButton.Equals("Start"))
        {
            if(CameraFix.sceneGradientIsStatus("SceneDark")) SceneManager.LoadScene(startScene);//新的加找場景方法
            else cameraObject.SendMessage("doChangeSceneOn");
        }
        else if (selectButton.Equals("Load"))
        {

        }
        else if (selectButton.Equals("Exit"))
        {
            if (CameraFix.sceneGradientIsStatus("SceneDark")) Application.Quit();//關閉遊戲
            else cameraObject.SendMessage("doChangeSceneOn");
        }
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
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit()
    {
        if (tagIndex == 1) selectButton = "Start";//Start
        else if (tagIndex ==2) doGameLoad();//Load
        else if (tagIndex ==3) selectButton = "Exit";//Exit
    }
    #endregion

    #region 讀取遊戲
    public void doGameLoad()
    {
        Debug.Log("doGameLoad");
        selectButton = "Load";
    }
    #endregion
}
