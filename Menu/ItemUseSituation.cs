using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseSituation : MonoBehaviour
{
    [Header("使用的道具名稱")]
    public string ItemUseName;

    [Header("使用道具的情境編號")]
    [Range(0, 15)]
    public int ItemUseSituationNum = 0;

    void Start()
    {
        transform.tag = "ItemUseSituation";//更改自身tag
    }

    void Update()
    {
        
    }
}
