using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagSave : MonoBehaviour
{
    //存檔菜單文字物件
    GameObject savePage;

    //存檔菜單文字的動畫信息
    private Animator savePageAanimator;

    //Menu物件
    GameObject MenuObject;

    //指標物件
    GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    void Start()
    {
        //取得savePage物件及動畫信息
        savePage = GameObject.Find("SavePage");
        savePageAanimator = savePage.GetComponent<Animator>();
        //取得Menu物件
        MenuObject = this.transform.parent.parent.gameObject;
        //取得指標物件
        itemIndex = this.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        //取得最大位置編號，要去掉指標物件
        tagIndexMax = this.transform.GetChild(2).gameObject.transform.childCount - 1;
        //初始化指標物件的位置
        Vector3 choicePosition = this.transform.GetChild(2).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.2F, choicePosition.y, choicePosition.z);
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        if (PlayerItemMenu.openDetailMenu == 2)
        {
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetKeyDown(KeyCode.Z) && PlayerItemMenu.overRestTime) doSubmit();
        }

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

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//第一項，向上移動
        else if (v > 0 && tagIndex == 1) tagIndex = tagIndexMax;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
        else if (v < 0 && tagIndex == tagIndexMax) tagIndex = 1;//最後一項，向下移動
        //更新指標位置
        Vector3 choicePosition = this.transform.GetChild(2).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.2F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit()
    {
        Debug.Log("Save_doSubmit()");
        PlayerItemMenu.returnRestTimer();
    }
    #endregion
}
