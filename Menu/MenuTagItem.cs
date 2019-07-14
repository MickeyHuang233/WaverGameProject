using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    菜單，物品欄畫面
    掛載物件：
        MenuTagItem --> ItemPage
    實現功能：
        1. 控制動畫的開啟和關閉
        2. 使用道具【未完成】
        3. 返回上一層菜單
*/
public class MenuTagItem : MonoBehaviour
{
    //物品欄文字物件
    GameObject itemPage;

    //指標物件
    GameObject itemIndex;

    //物品說明物件
    GameObject descriptionObject;

    //當前位置編號
    private int tagIndex = 1;

    //最大位置編號
    private int tagIndexMax;

    #region Start()
    void Start()
    {
        //取得itemPage物件及動畫信息
        itemPage = GameObject.Find("ItemPage");
        //取得指標物件
        itemIndex = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        //取得物品說明物件
        descriptionObject = transform.GetChild(0).gameObject;
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
        if (PlayerItemMenu.openDetailMenu == 1)
        {
            if (Input.GetButtonDown("Vertical")) doMove(v);
            if (Input.GetButtonDown("Submit") && PlayerItemMenu.overRestTime) doSubmit();
        }
        }
    #endregion

    #region 顯示物品欄中的物品名稱
    private void showItemName()
    {
        //顯示與所擁有物品數相當的欄位數
        GameObject ItemObject = transform.GetChild(1).gameObject;
        for (int i = 1; i <= 5; i++)
        {
            ItemObject.transform.GetChild(i).gameObject.transform.localScale = new Vector3((i > GameMenager.getItemNumList.Count) ? 0F : 1F, 1F, 1F);
        }
        int j = 1;
        foreach (int itemNum in GameMenager.getItemNumList)
        {
            //Debug.Log(ItemObject.transform.GetChild(j).GetComponent<Text>().text);
            ItemObject.transform.GetChild(j).GetComponent<Text>().text = GameMenager.itemInformationList[itemNum].ItemName;
            j++;
        }
        //更新指標能走的最大位置編號
        tagIndexMax = GameMenager.getItemNumList.Count;
    }
    #endregion

    #region 顯示指定物品的物品說明 showItemDescription(int itemNum)
    private void showItemDescription(int itemNum)
    {
        descriptionObject.GetComponent<Text>().text = GameMenager.itemInformationList[itemNum].ItemDescription;
    }
    #endregion

    #region 打開物品欄
    private void showPage()
    {
        showItemName();
        //如果物品欄有東西，就顯示第一個物品的物品說明
        if (GameMenager.getItemNumList.Count > 0) showItemDescription(0);
    }
    #endregion

    #region 關閉物品欄
    private void hidePage()
    {
        //將指標返回至第一個物品的位置
        tagIndex = 1;
        Vector3 choicePosition_02 = transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
    }
    #endregion

    #region 指標移動
    private void doMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//第一項，向上移動
        else if (v > 0 && tagIndex == 1) tagIndex = tagIndexMax;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
        else if (v < 0 && tagIndex == tagIndexMax) tagIndex = 1;//最後一項，向下移動
        //更新物品說明
        showItemDescription(tagIndex - 1);
        //更新指標位置
        Vector3 choicePosition = this.transform.GetChild(1).gameObject.transform.GetChild(tagIndex).gameObject.transform.position;
        itemIndex.transform.position = new Vector3(choicePosition.x - 0.25F, choicePosition.y, choicePosition.z);
    }
    #endregion

    #region 按下確認鍵操作
    private void doSubmit()
    {
        PlayerItemMenu.returnRestTimer();
        StartCoroutine(doCloseAllMenu());//協程_關閉菜單
    }
    #endregion

    #region 協程_關閉菜單
    IEnumerator doCloseAllMenu()
    {
        GameObject.Find("Menu").SendMessage("doCloseDetailMenu");
        yield return new WaitForSeconds(0.2F);
        GameObject.Find("Menu").SendMessage("doCloseMenu");

        yield return new WaitForSeconds(0.2F);
        List<int> itemNums = new List<int>();
        foreach (int itemNum in GameMenager.getItemNumList) itemNums.Add(itemNum);
        GameObject useItemameObject = GameObject.Find(GameMenager.itemInformationList[itemNums[tagIndex - 1]].UseItemameObject);

        if (useItemameObject != null)
        {
            useItemameObject.SendMessage("itemUse");
        }
        else//場景無此物件時
        {
            Debug.Log("此場景無此物件可以使用");
        }
    }
    #endregion
}
