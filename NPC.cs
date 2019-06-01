using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

//可調查NPC的操作
public class NPC : MonoBehaviour
{

    private Flowchart npcFlowchart;

    void Start()
    {
        this.transform.tag = "NPC";//更改自身tag
        npcFlowchart = this.GetComponent<Flowchart>();
    }
    
    void Update()
    {
        
    }

    public void npcTalking()
    {
        Block enterBlock = npcFlowchart.FindBlock("NPC_Fungus_Enter");
        npcFlowchart.ExecuteBlock(enterBlock);
    }

    //標示NPC
    private void doHightLineOn()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        // TODO Portal，當NPC可被調查時應該要如何實現標示
    }

    //取消標示NPC
    private void doHightLineOff()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        // TODO Portal，當NPC取消標示的實現，應與實現相對應
    }

    private void chackItem()
    {
        GameMenager.chackItem();
    }

    private void getItem()
    {
        GameMenager.getItem(1);
    }
}
