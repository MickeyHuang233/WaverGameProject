using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

//可調查物品的操作
public class NPC_Item : MonoBehaviour
{
    private Flowchart npcFlowchart;

    void Start()
    {
        transform.tag = "NPC_Item";//更改自身tag
        npcFlowchart = GetComponent<Flowchart>();
    }
    
    void Update()
    {

    }

    #region 調查物品      itemTalking()
    public void itemTalking()
    {
        Block enterBlock = npcFlowchart.FindBlock("NPC_Fungus_Enter");
        npcFlowchart.ExecuteBlock(enterBlock);
    }
    #endregion

    //標示Item
    private void doHightLineOn()
    {
        SendMessage("OnEnable");
    }

    //取消標示Item
    private void doHightLineOff()
    {
        SendMessage("OnDisable");
    }
}
