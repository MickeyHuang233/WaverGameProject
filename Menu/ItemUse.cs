using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ItemUse : MonoBehaviour
{
    //自身對話插件
    private Flowchart itemUseFlowchart;

    #region Start()
    void Start()
    {
        transform.tag = "ItemUse";//更改自身tag
        itemUseFlowchart = GetComponent<Flowchart>();
    }
    #endregion

    #region 道具使用      itemUse()
    public void itemUse()
    {
        //使用道具的分歧編號，0代表找不到相應的使用地點
        int useItemRole = (SurveyJudge.ItemUseMap.ContainsKey(transform.name)) ? SurveyJudge.ItemUseMap[transform.name] : 0;
        itemUseFlowchart.SetIntegerVariable("useItemRole", useItemRole);//設置分歧編號
        //進入統一使用道具的入口
        Block enterBlock = itemUseFlowchart.FindBlock("UseItem_Enter");
        itemUseFlowchart.ExecuteBlock(enterBlock);
    }
    #endregion
}
