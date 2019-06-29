using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    視角相關的操作
    掛載物件：
        CameraFix --> Main Camera
    使用插件：
        Cinemachine
    實現功能：
        1. 場景淡入淡出效果
        2. 顯示場景名稱
        3. 顯示倒數時間
*/
public class CameraFix : MonoBehaviour
{
    //場景漸變物件的動畫組件
    public static Animator sceneGradientAnimator;

    //顯示場景名稱物件
    public static GameObject sceneNameObject;

    //顯示場景名稱物件的動畫組件
    public static Animator sceneNameAnimator;

    //顯示倒數時間物件
    public static GameObject nagativeTimerObject;

    //顯示倒數時間物件的動畫組件
    public static Animator nagativeTimerAnimator;

    #region Start()
    void Start()
    {
        //獲取場景漸變物件的動畫組件
        sceneGradientAnimator = GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Animator>();
        //獲取場景名稱顯示物件
        sceneNameObject = GameObject.Find("Canvas_UI").transform.GetChild(1).gameObject;
        sceneNameAnimator = sceneNameObject.GetComponent<Animator>();
        //獲取顯示倒數時間物件
        nagativeTimerObject = GameObject.Find("Canvas_UI").transform.GetChild(2).gameObject;
        nagativeTimerAnimator = nagativeTimerObject.GetComponent<Animator>();
    }
    #endregion

    #region 場景淡出
    public static void doChangeSceneOn()
    {
        InitSceneManagment.isNeedScenceDark = true;
        sceneGradientAnimator.SetBool("changeScene_On", true);
        sceneGradientAnimator.SetBool("changeScene_Off", false);
    }
    #endregion

    #region 場景淡入
    public static void doChangeSceneOff()
    {
        InitSceneManagment.isNeedScenceDark = false;
        sceneGradientAnimator.SetBool("changeScene_On", false);
        sceneGradientAnimator.SetBool("changeScene_Off", true);
    }
    #endregion

    #region 顯示場景名稱   showSceneName(string sceneName)
    public static void showSceneName(string sceneName)
    {
        sceneNameObject.GetComponent<Text>().text = sceneName;
        sceneNameAnimator.SetTrigger("showSceneName");
    }
    #endregion

    #region 倒計時時顯示剩余時間   showNagativeTime(string timer)
    public static void showNagativeTime(string timer)
    {
        nagativeTimerObject.GetComponent<Text>().text = timer;
        nagativeTimerAnimator.SetBool("showTimer", (GameTimer.nactivesecond > 0) ? true: false);//一般顯示
        nagativeTimerAnimator.SetBool("hideTimer", (GameTimer.nactivesecond > 0) ? false : true);//隱藏
        nagativeTimerAnimator.SetBool("showRed", (GameTimer.nactivesecond > 10) ? false : true);//警告
    }
    #endregion

    #region 判斷漸變物件當前狀態  sceneGradientIsStatus(string status)
    public static bool sceneGradientIsStatus(string status)
    {
        bool b = false;
        AnimatorStateInfo currentState = sceneGradientAnimator.GetCurrentAnimatorStateInfo(0);//取得當前動畫狀態
        switch (status)
        {
            case "SceneDark":
                b = currentState.IsName("SceneDark");//淡出
                break;
            case "SceneWhite":
                b = currentState.IsName("SceneWhite");//淡入
                break;
            default:
                Debug.Log(status + "找不到相對應的狀態，可能輸入錯誤");
                break;
        }
        return b;
    }
    #endregion
}
