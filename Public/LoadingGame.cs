using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//開炲遊戲和讀取遊戲的變量操作
public class LoadingGame : MonoBehaviour
{
    [Header("初始化完成後要轉入的場景")]
    public string startScene;

    [Header("初始化完成後要轉入的場景傳送位置編號")]
    public string startPositionnName;

    #region Start()
    void Start()
    {
        if (TitleSceneControl.selectButton.Equals("Start")) setStartVar();
        else if (TitleSceneControl.selectButton.Equals("Load")) setLoadVar(TitleSceneControl.loadIndex);
    }
    #endregion


    #region 開始遊戲的變量操作
    private void setStartVar()
    {
        SceneManager.LoadScene(startScene);
        InitSceneManagment.targetPositionName = startPositionnName;//傳送位置物件名稱，以便新場景找到相應的物件
    }
    #endregion

    #region 讀取遊戲的變量操作
    private void setLoadVar(int saveIndex)
    {

    }
    #endregion
}
