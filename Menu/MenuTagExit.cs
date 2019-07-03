using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    菜單，離開遊戲畫面
    掛載物件：
        MenuTagExit --> ExitPage
    實現功能：
        1. 控制動畫的開啟和關閉
        2. 離開遊戲
        3. 返回上一層菜單
*/
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
        if (PlayerItemMenu.openDetailMenu == 3)
        {
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetButtonDown("Submit") && PlayerItemMenu.overRestTime) doSubmit();
        }
    }
    #endregion

    #region 打開離開遊戲菜單
    private void showPage()
    {

    }
    #endregion

    #region 關閉離開遊戲菜單
    private void hidePage()
    {
        PlayerItemMenu.openDetailMenu = 0;
        tagIndex = 2;//重置位置編號
        Vector3 choicePosition_02 = transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
        PlayerItemMenu.returnRestTimer();
    }
    #endregion

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//第一項，向上移動
        else if (v > 0 && tagIndex == 1) tagIndex = tagIndexMax;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
        else if (v < 0 && tagIndex == tagIndexMax) tagIndex = 1;//最後一項，向下移動
        Vector3 choicePosition = this.transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit()
    {
        PlayerItemMenu.returnRestTimer();
        switch (tagIndex)
        {
            case 1://是的
                Application.Quit();//關閉遊戲
                break;
            case 2://取消
                hidePage();
                break;
            }
    }
    #endregion
}
