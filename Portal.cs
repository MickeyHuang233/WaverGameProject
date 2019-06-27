using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//角色傳送元件
public class Portal : MonoBehaviour
{
    [Header("場景名稱")]
    public string targetScenceName;

    [Header("傳送位置物件名稱")]
    public string targetPositionName;
    
    void Start()
    {
        this.transform.tag = "Portal";//更改自身tag
    }

    public void ChangeScene()
    {
        InitSceneManagment.targetSceneName = targetScenceName;//設置目標場景
        InitSceneManagment.targetPositionName = targetPositionName;//傳送位置物件名稱，以便新場景找到相應的物件
    }

    //標示Portal
    private void doHightLineOn()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        // TODO Portal，當傳送門可被調查時應該要如何實現標示
    }

    //取消標示Portal
    private void doHightLineOff()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        // TODO Portal，當傳送門取消標示的實現，應與實現相對應
    }

}
