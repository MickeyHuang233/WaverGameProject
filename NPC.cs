using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可調查NPC的操作
public class NPC : MonoBehaviour
{
    void Start()
    {
        this.transform.tag = "NPC";//更改自身tag
    }
    
    void Update()
    {
        
    }

    //標示NPC
    public void doHightLineOn()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        // TODO Portal，當NPC可被調查時應該要如何實現標示
    }

    //取消標示NPC
    public void doHightLineOff()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        // TODO Portal，當NPC取消標示的實現，應與實現相對應
    }
}
