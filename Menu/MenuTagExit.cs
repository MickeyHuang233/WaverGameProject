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
        //初始化指標物件的位置
        Vector3 choicePosition_02 = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        if (PlayerItemMenu.overRestTime && PlayerItemMenu.openDetailMenu == 3)
        {
            if (v < 0 && tagIndex == 1)
            {
                Vector3 choicePosition_02 = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.position;
                itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
                tagIndex++;
            } 
            else if (v > 0 && tagIndex == 2)
            {
                Vector3 choicePosition_01 = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.position;
                itemIndex.transform.position = new Vector3(choicePosition_01.x - 0.25F, choicePosition_01.y, choicePosition_01.z);
                tagIndex--;
            }
            if (submit > 0)
            {
                PlayerItemMenu.returnRestTimer();
                switch (tagIndex)
                {
                    case 1://是的
                        doExitSubmit();
                        break;
                    case 2://取消
                        MenuObject.SendMessage("doCloseDetailMenu");
                        break;
                }
            }
        }
    }
    #endregion

    # region 打開離開遊戲菜單
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

    #region 確認離開遊戲
    private void doExitSubmit()
    {
            Debug.Log("doExitSubmit");
            Application.Quit();//關閉遊戲
    }
    #endregion
}
