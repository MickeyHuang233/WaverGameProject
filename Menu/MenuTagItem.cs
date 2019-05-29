using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagItem : MonoBehaviour
{
    //物品欄文字物件
    GameObject itemPage;

    //物品欄文字的動畫信息
    private Animator itemPageAanimator;

    #region Start()
    void Start()
    {
        itemPage = GameObject.Find("ItemPage");
        itemPageAanimator = itemPage.GetComponent<Animator>();
    }
    #endregion

    #region Update()
    void Update()
    {

    }
    #endregion

    #region 打開物品欄
    private void showItemPage()
    {
        itemPageAanimator.SetBool("openDetilMenu", true);
        itemPageAanimator.SetBool("closeDetilMenu", false);
    }
    #endregion

    #region 關閉物品欄
    private void hideItemPage()
    {
        itemPageAanimator.SetBool("openDetilMenu", false);
        itemPageAanimator.SetBool("closeDetilMenu", true);
    }
    #endregion
}
