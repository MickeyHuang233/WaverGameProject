using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTagItem : MonoBehaviour
{
    //物品欄文字物件
    GameObject itemPage;

    //物品欄文字的動畫信息
    private Animator itemPageAanimator;

    //Menu物件
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
        if (PlayerItemMenu.openDetailMenu == 1)
        {
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetKeyDown(KeyCode.Z) && PlayerItemMenu.overRestTime) doSubmit();
        }

        }
    #endregion

    #region 打開物品欄
    private void showItemPage()
    {
        itemPageAanimator.SetBool("openDetilMenu", true);
        itemPageAanimator.SetBool("closeDetilMenu", false);
        showItemName();
    }
    #endregion

    #region 顯示物品欄中的物品名稱
    private void showItemName()
    {
        //顯示與所擁有物品數相當的欄位數
        GameObject ItemObject = this.transform.GetChild(1).gameObject;
        for (int i = 1; i <= 5; i++)
        {
            ItemObject.transform.GetChild(i).gameObject.transform.localScale = new Vector3((i > GameMenager.getItemNumList.Count) ? 0F : 1F, 1F, 1F);
        }
        int j = 1;
        foreach (int ItemNum in GameMenager.getItemNumList)
        {
            //Debug.Log(ItemObject.transform.GetChild(j).GetComponent<Text>().text);
            ItemObject.transform.GetChild(j).GetComponent<Text>().text = GameMenager.itemInformationList[ItemNum].ItemName;
            j++;
        }
        //更新指標能走的最大位置編號
        tagIndexMax = GameMenager.getItemNumList.Count;
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
        Debug.Log("Item_doSubmit()");
        PlayerItemMenu.returnRestTimer();
    }
    #endregion
}
