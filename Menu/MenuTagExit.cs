using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //指標物件
    GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 2;

    //最大位置編號
    private int tagIndexMax;

    [Header("返回主頁面時需要銷毀的物件名稱")]
    public string[] destoryObjectNames;

    #region Start()
    void Start()
    {
        //取得指標物件
        itemIndex = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = transform.GetChild(1).gameObject.transform.childCount - 1;
        //初始化指標物件的位置
        Vector3 choicePosition = transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
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
        Vector3 choicePosition = transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
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
                StartCoroutine(doRetrunTitle());//協程_返回主菜單
                break;
            case 2://取消
                hidePage();
                break;
            }
    }
    #endregion

    #region 協程_返回主菜單
    IEnumerator doRetrunTitle()
    {
        GameObject.Find("Menu").SendMessage("doCloseDetailMenu");
        yield return new WaitForSeconds(0.2F);

        GameObject.Find("Menu").SendMessage("doCloseMenu");
        yield return new WaitForSeconds(0.2F);

        foreach (string destoryObjectName in destoryObjectNames)
        {
            Destroy(GameObject.Find(destoryObjectName));
        }
        SceneManager.LoadScene("00_Title");//新的加找場景方法
        //Application.Quit();//關閉遊戲
    }
    #endregion
}
