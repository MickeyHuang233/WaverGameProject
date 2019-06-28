using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//視角相關的操作
public class CameraFix : MonoBehaviour
{
    [Header("上下偏移單位")]
    [Range(-5F, 5F)]
    public float cameraYPlus = 0F;

    [Header("左右偏移單位")]
    [Range(-5F, 5)]
    public float cameraXPlus = 0F;

    [Header("要跟隨的玩家物件")]
    public GameObject player;

    //Camara插件的Compoment
    CinemachineVirtualCamera cinemachineVirtualCamera;

    //關閉菜單時的Camara距離，在CM vcam1中指定
    private float closeMenuOrthographicSize;

    [Header("打開菜單時的Camara距離")]
    [Range(0F, 5F)]
    public float openMenuOrthographicSize = 0.8F;

    [Header("打開菜單時的緩慢移動區Y軸偏移量")]
    [Range(0F, 5F)]
    public float openMenuOrthographicY = 0.8F;

    [Header("打開及關閉菜單視角的移動速度")]
    [Range(0F, 10F)]
    public float targetMenuSpeed = 5F;

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

    private float ShakeElapsedTime = 0F;

    [Header("視角晃動強度")]
    [Range(-1F, 1F)]
    public float ShakeAmplitude = 0.05F;

    [Header("視角晃動頻率")]
    [Range(-1F, 1F)]
    public float ShakeFrequency = 0.2F;

    //實現視角晃動的物件
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

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

    #region Update()
    void Update()
    {

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
        nagativeTimerAnimator.SetBool("showTimer", (GameTimer.nactivesecond > 0) ? true: false);
        nagativeTimerAnimator.SetBool("hideTimer", (GameTimer.nactivesecond > 0) ? false : true);
        nagativeTimerAnimator.SetBool("showRed", (GameTimer.nactivesecond > 10) ? false : true);
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
                b = currentState.IsName("SceneDark");
                break;
            case "SceneWhite":
                b = currentState.IsName("SceneWhite");
                break;
            default:
                Debug.Log(status + "找不到相對應的狀態，可能輸入錯誤");
                break;
        }
        return b;
    }
    #endregion
}
