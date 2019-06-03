using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //關閉菜單時的緩慢移動區Y軸偏移量，在CM vcam1中指定
    private float closeMenuOrthographicY = 0F;

    [Header("打開菜單時的緩慢移動區Y軸偏移量")]
    [Range(0F, 5F)]
    public float openMenuOrthographicY = 0.8F;

    [Header("打開及關閉菜單視角的移動速度")]
    [Range(0F, 10F)]
    public float targetMenuSpeed = 5F;

    #region Start()
    void Start()
    {
        //獲取Camara插件的Compoment
        cinemachineVirtualCamera = GameObject.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>();
        //獲取關閉菜單時的Camara距離
        closeMenuOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }
    #endregion

    #region Update()
    void Update()
    {
        //視角移動
        moveCamera();

    }
    #endregion

    #region 根據菜單狀態進行視角移動
    private void moveCamera()
    {
        //調整放大倍數
        float targetOrthographicSize = (PlayerItemMenu.openDetailMenu == -1) ? closeMenuOrthographicSize : openMenuOrthographicSize;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * targetMenuSpeed);
        float targetOrthographicY = (PlayerItemMenu.openDetailMenu == -1) ? closeMenuOrthographicY : closeMenuOrthographicY = cinemachineVirtualCamera.State.RawPosition.y + openMenuOrthographicY;

        #endregion
    }
}
