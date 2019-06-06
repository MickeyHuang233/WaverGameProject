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

    //場景名稱顯示物件
    public static GameObject sceneNameObject;

    //場景名稱顯示物件的動畫組件
    public static Animator sceneNameAnimator;

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
        //獲取場景名稱顯示物件的動畫組件
        sceneNameAnimator = sceneNameObject.GetComponent<Animator>();
        //獲取Camara插件的Compoment
        cinemachineVirtualCamera = GameObject.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>();
        //獲取實現視角晃動的物件
        virtualCameraNoise = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        //獲取關閉菜單時的Camara距離
        closeMenuOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }
    #endregion

    #region Update()
    void Update()
    {
        //視角移動
        moveCamera();
        //晃動相機
        if(player != null)
        {
            AnimatorStateInfo currentState = player.transform.GetChild(3).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);//取得Menu當前動畫狀態
            bool isShaking = (PlayerItemMenu.openDetailMenu > -1) ? true : false;
            shakeCamera(isShaking);
        }

    }
    #endregion

    #region 晃動相機    shakeCamera()
    private void shakeCamera(bool isShaking)
    {
        if (cinemachineVirtualCamera != null && virtualCameraNoise != null)
        {
            if (isShaking)
            {
                virtualCameraNoise.m_AmplitudeGain = (virtualCameraNoise.m_AmplitudeGain >= (ShakeAmplitude * 0.8F)) ? Mathf.Lerp(virtualCameraNoise.m_AmplitudeGain, ShakeAmplitude, Time.deltaTime * 0.5F) : ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = (virtualCameraNoise.m_FrequencyGain >= (ShakeFrequency * 0.8F)) ? Mathf.Lerp(virtualCameraNoise.m_FrequencyGain, ShakeFrequency, Time.deltaTime * 0.5F) : ShakeAmplitude;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = (virtualCameraNoise.m_AmplitudeGain >= 0.01F)? Mathf.Lerp(virtualCameraNoise.m_AmplitudeGain, 0F, Time.deltaTime * 2F) : 0F;
                virtualCameraNoise.m_FrequencyGain = (virtualCameraNoise.m_FrequencyGain >= 0.01F) ? Mathf.Lerp(virtualCameraNoise.m_FrequencyGain, 0F, Time.deltaTime * 2F) : 0F;
            }
        }
    }
    #endregion

    #region 根據菜單狀態進行視角移動
    private void moveCamera()
    {
        //調整放大倍數
        float targetOrthographicSize = (PlayerItemMenu.openDetailMenu == -1) ? closeMenuOrthographicSize : openMenuOrthographicSize;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * targetMenuSpeed);
    }
    #endregion

    #region 場景淡出
    private void doChangeSceneOn()
    {
        InitSceneManagment.isNeedScenceDark = true;
        sceneGradientAnimator.SetBool("changeScene_On", true);
        sceneGradientAnimator.SetBool("changeScene_Off", false);
    }
    #endregion

    #region 場景淡入
    private void doChangeSceneOff()
    {
        InitSceneManagment.isNeedScenceDark = false;
        sceneGradientAnimator.SetBool("changeScene_On", false);
        sceneGradientAnimator.SetBool("changeScene_Off", true);
    }
    #endregion

    #region 顯示場景名稱   showSceneName()
    public static void showSceneName(string sceneName)
    {
        sceneNameObject.GetComponent<Text>().text = sceneName;
        sceneNameAnimator.SetTrigger("showSceneName");
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
