using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagSave : MonoBehaviour
{
    //存檔菜單文字物件
    GameObject savePage;

    //存檔菜單文字的動畫信息
    private Animator savePageAanimator;

    void Start()
    {
        savePage = GameObject.Find("SavePage");
        savePageAanimator = savePage.GetComponent<Animator>();
    }

    void Update()
    {

    }

    //打開存檔菜單
    private void showSavePage()
    {
        savePageAanimator.SetBool("openDetilMenu", true);
        savePageAanimator.SetBool("closeDetilMenu", false);
    }

    //關閉存檔菜單
    private void hideSavePage()
    {
        savePageAanimator.SetBool("openDetilMenu", false);
        savePageAanimator.SetBool("closeDetilMenu", true);
    }
}
