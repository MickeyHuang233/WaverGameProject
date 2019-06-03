using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //物品編號
    private int itemId;
    public int ItemId { get; set; }

    //物品名稱
    private string itemName;
    public string ItemName { get; set; }

    //物品說明
    private string itemDescription;
    public string ItemDescription { get; set; }

    //物品說明_在地下室才顯示
    private string itemDescriptionbBasement;
    public string ItemDescriptionbBasement { get; set; }

    //物品使用域判斷的物件名
    private string useItemameObject;
    public string UseItemameObject { get; set; }

}
