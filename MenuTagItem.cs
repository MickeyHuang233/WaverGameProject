using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagItem : MonoBehaviour
{
    //物品欄文字物件
    GameObject itemPage;

    //物品欄文字的動畫信息
    private Animator itemPageAanimator;

    void Start()
    {
        itemPage = GameObject.Find("ItemPage");
        itemPageAanimator = itemPage.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    //打開物品欄
    private void showItemPage()
    {
        Debug.Log("MenuTag_Item_showItemPage");
        itemPageAanimator.SetBool("openDetilMenu", true);
        itemPageAanimator.SetBool("closeDetilMenu", false);
    }

    //關閉物品欄
    private void hideItemPage()
    {
        Debug.Log("MenuTag_Item_hideItemPage");
        itemPageAanimator.SetBool("openDetilMenu", false);
        itemPageAanimator.SetBool("closeDetilMenu", true);
    }
}
