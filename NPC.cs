using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

//可調查NPC的操作
public class NPC : MonoBehaviour
{
    private Flowchart npcFlowchart;

    #region Start()
    void Start()
    {
        transform.tag = "NPC";//更改自身tag
        npcFlowchart = GetComponent<Flowchart>();
    }
    #endregion

    #region Update()
    void Update()
    {

    }
    #endregion

    #region 與NPC對話      npcTalking()
    public void npcTalking()
    {
        Block enterBlock = npcFlowchart.FindBlock("NPC_Fungus_Enter");
        npcFlowchart.ExecuteBlock(enterBlock);
    }
    #endregion

    #region 標示NPC      doHightLineOn()
    private void doHightLineOn()
    {
        SendMessage("showTalkBubble_01");
    }
    #endregion

    #region 取消標示NPC      doHightLineOff()
    private void doHightLineOff()
    {
        SendMessage("hideTalkBubble");
    }
    #endregion

    #region 打印玩家擁有的道具      chackItem()
    private void chackItem()//暫時使用，未來可能刪掉
    {
        GameMenager.chackItem();
    }
    #endregion

    #region 獲得道具      getItem()
    private void getItem()//暫時做法
    {
        GameMenager.getItem(1);
    }
    #endregion
}
