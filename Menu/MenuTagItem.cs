using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagItem : MonoBehaviour
{
    //物品欄文字物件
    GameObject itemPage;

    //物品欄文字的動畫信息
    private Animator itemPageAanimator;

    GameObject MenuObject;

    //指標物件
    GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    #region Start()
    void Start()
    {
        //取得itemPage物件及動畫信息
        itemPage = GameObject.Find("ItemPage");
        itemPageAanimator = itemPage.GetComponent<Animator>();
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
        if (PlayerItemMenu.overRestTime && PlayerItemMenu.openDetailMenu == 1)
        {
            if(v != 0) doMove(v);
            if (submit > 0) doSubmit();
        }

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

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1)//向上移動
        {
            tagIndex--;
        }
        else if (v > 0 && tagIndex == 1)//向上移動
        {
            tagIndex = tagIndexMax;
        }
        else if (v < 0 && tagIndex < tagIndexMax)//向下移動
        {
            tagIndex++;
        }
        else if (v < 0 && tagIndex == tagIndexMax)//向下移動
        {
            tagIndex = 1;
        }
        Vector3 choicePosition = this.transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
        PlayerItemMenu.returnRestTimer();
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit()
    {
        Debug.Log("Item_doSubmit()");
        PlayerItemMenu.returnRestTimer();
    }
    #endregion
}
