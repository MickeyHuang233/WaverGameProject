using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDarkLayoutMove : MonoBehaviour
{
    //用來管理DarkLayout的物件(父類)
    private GameObject darkLayoutObject;

    //用來管理DarkLayout的物件(子類)
    private List<GameObject> darkLayoutObjects;

    //玩家物件
    private GameObject playerObject;

    //目標位置
    Vector3 targetPosition;

    [Header("視角移動速度")]
    [Range(0F, 50F)]
    public float moveSpeed;

    void Start()
    {
        //取得玩家物件
        playerObject = GameObject.FindGameObjectWithTag("Player");

        //將DarkLayout的全部子物件放入List中
        darkLayoutObject = GameObject.Find("DarkLayout");
        Debug.Log(darkLayoutObject.transform.GetChildCount());
        for (int i = 0; i < darkLayoutObject.transform.GetChildCount(); i++)
        {
            GameObject d = gameObject.transform.GetChild(i).gameObject;
            darkLayoutObjects.Add(d);
        }
    }

    void Update()
    {
        float playerX = playerObject.transform.position.x;
        float playerY = playerObject.transform.position.y;

        for (int i = 0; i < darkLayoutObjects.Count; i++)
        {
            GameObject d = darkLayoutObjects[i];
            targetPosition = new Vector3(playerX, playerY, 0);
            d.transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
