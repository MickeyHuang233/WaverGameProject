using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagExit : MonoBehaviour
{
    //離開遊戲菜單文字物件
    GameObject exitPage;

    //離開遊戲菜單文字的動畫信息
    private Animator exitPageAanimator;

    void Start()
    {
        exitPage = GameObject.Find("ExitPage");
        exitPageAanimator = exitPage.GetComponent<Animator>();
    }

    void Update()
    {

    }

    //打開離開遊戲菜單
    private void showExitPage()
    {
        Debug.Log("MenuTag_Exit_showItemPage");
        exitPageAanimator.SetBool("openDetilMenu", true);
        exitPageAanimator.SetBool("closeDetilMenu", false);
    }

    //關閉離開遊戲菜單
    private void hideExitPage()
    {
        Debug.Log("MenuTag_Exit_hideItemPage");
        exitPageAanimator.SetBool("openDetilMenu", false);
        exitPageAanimator.SetBool("closeDetilMenu", true);
    }

    //確認離開遊戲
    private void doExitSubmit()
    {

    }

    //取消離開遊戲
    private void doExitCancel()
    {

    }
}
