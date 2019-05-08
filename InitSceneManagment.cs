﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//每一個場景初始化的操作，一定要每個場景都加，否則無法傳送到指定位置
public class InitSceneManagment : MonoBehaviour
{
    //要傳送的位置名
    static string positionName;

    //玩家物件
    private GameObject playerObject;

    //用來管理DarkLayout的物件
    private Transform darkLayoutHolder;

    [Header("地圖類型")]
    public string mapType;

    [Header("1F顯示物件")]
    public GameObject[] darkLayout_1F;

    //傳送位置物件名稱，主要從Portal賦值
    public static string PositionName { get => positionName; set => positionName = value; }

    void Start()
    {
        //取得玩家物件
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.Log("Tag 'Player' is not found!!");
            return;//找不到玩家物件直接返回
        }

        //取得指定傳送點物件
        GameObject targetPositionObject = GameObject.Find(positionName) as GameObject;
        if (targetPositionObject != null)
        {
            playerObject.transform.position = targetPositionObject.transform.position;
        }
        else
        {
            Debug.Log(positionName + " is not found!!");
        }

        //建立前景黑的物件
        createDarkLayout();
    }

    //建立前景黑的物件
    private void createDarkLayout()
    {
        darkLayoutHolder = new GameObject("DarkLayout").transform;//建立一個新的物件叫Map，並取得這個物件的transform屬性
        if (mapType.Equals("1F"))//依地圖類型分産出的DarkLayout物件，默認是什麼都不産出
        {
            for (int i = 0; i < darkLayout_1F.Length; i++)
            {
                GameObject go = GameObject.Instantiate(darkLayout_1F[i], new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(darkLayoutHolder);//將新建出來的地圖放到darkLayoutHolder物件底下
            }
        }
        else if (mapType.Equals("3F"))
        {

        }
        else if (mapType.Equals("B1"))
        {

        }
    }

}
