using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagExit : MonoBehaviour
{
    //離開遊戲菜單文字物件
    GameObject exitPage;

    //離開遊戲菜單文字的動畫信息
    private Animator exitPageAanimator;

    GameObject MenuObject;

    //指標物件
    GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 2;

    //最大位置編號
    private int tagIndexMax;

    #region Start()
    void Start()
    {
        //取得exitPage物件及動畫信息
        exitPage = GameObject.Find("ExitPage");
        exitPageAanimator = exitPage.GetComponent<Animator>();
        //取得Menu物件
        MenuObject = this.transform.parent.parent.gameObject;
        //取得指標物件
        itemIndex = this.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = this.transform.GetChild(1).gameObject.transform.childCount - 1;
        //初始化指標物件的位置
        Vector3 choicePosition = this.transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        if (PlayerItemMenu.overRestTime && PlayerItemMenu.openDetailMenu == 3)
        {
            if (v != 0) doMove(v);
            if (submit > 0) doSubmit(submit);
        }
    }
    #endregion

    #region 打開離開遊戲菜單
    private void showExitPage()
    {
        exitPageAanimator.SetBool("openDetilMenu", true);
        exitPageAanimator.SetBool("closeDetilMenu", false);
    }
    #endregion

    #region 關閉離開遊戲菜單
    private void hideExitPage()
    {
        exitPageAanimator.SetBool("openDetilMenu", false);
        exitPageAanimator.SetBool("closeDetilMenu", true);
        //將指標返回至"取消"狀態上
        tagIndex = 2;
        Vector3 choicePosition_02 = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
    }
    #endregion

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1)//向上移動
        {
            tagIndex--;
        }
        else if (v < 0 && tagIndex < tagIndexMax)//向下移動
        {
            tagIndex++;
        }
        Vector3 choicePosition = this.transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
        PlayerItemMenu.returnRestTimer();
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit(float submit)
    {
        if (submit > 0)
        {
            PlayerItemMenu.returnRestTimer();
            switch (tagIndex)
            {
                case 1://是的
                    Application.Quit();//關閉遊戲
                    break;
                case 2://取消
                    MenuObject.SendMessage("doCloseDetailMenu");
                    break;
            }
        }
    }
    #endregion
}
