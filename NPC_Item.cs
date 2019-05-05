using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可調查物品的操作
public class NPC_Item : MonoBehaviour
{
    void Start()
    {
        this.transform.tag = "NPC_Item";//更改自身tag
    }
    
    void Update()
    {

    }

    //標示Item
    public void doHightLineOn()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        // TODO Portal，當物品可被調查時應該要如何實現標示
    }

    //取消標示Item
    public void doHightLineOff()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        // TODO Portal，當物品取消標示的實現，應與實現相對應
    }
}
