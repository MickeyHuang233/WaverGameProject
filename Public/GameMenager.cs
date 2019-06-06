using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//在遊戲初始化場景使用，加載完共用的元件後馬上轉至下一個場景
public class GameMenager : MonoBehaviour
{
    [Header("初始化完成後要轉入的場景")]
    public string startScene;

    [Header("初始化完成後要轉入的場景傳送位置編號")]
    public string startPositionnName;

    //所有遊戲道具信息
    public static List<Item> itemInformationList;

    //所有遊戲場景信息
    public static List<Map> mapInformationList;

    [Header("遊戲初始獲得道具")]
    public List<int> gameInitGetItemList;

    //物品欄已獲得物品編號
    public static HashSet<int> getItemNumList;

    void Start()
    {
        ParseMapJSON();//讀取場景json定義檔
        ParseItemJSON();//讀取物品json定義檔
        initGetItem();//初始化獲得的道具
        SceneManager.LoadScene(startScene);
        InitSceneManagment.targetPositionName = startPositionnName;//傳送位置物件名稱，以便新場景找到相應的物件
    }

    #region 讀取物品json定義檔，放入bean  ParseItemJSON()
    private void ParseItemJSON()
    {
        itemInformationList = new List<Item>();
        TextAsset itemText = Resources.Load<TextAsset>("Json/ItemDefinition");
        if (itemText != null)
        {
            JSONObject itemJSON = new JSONObject(itemText.text);
            foreach (JSONObject itemInformation in itemJSON.list)
            {
                Item item = new Item
                {
                    ItemId = (int)itemInformation["itemId"].n,
                    ItemName = itemInformation["itemName"].str,
                    ItemDescription = itemInformation["itemDescription"].str,
                    ItemDescriptionbBasement = itemInformation["itemDescriptionbBasement"].str,
                    UseItemameObject = itemInformation["useItemameObject"].str
                };
                itemInformationList.Add(item);
            }
        }
    }
    #endregion

    #region 讀取場景json定義檔，放入bean  ParseMapJSON()
    private void ParseMapJSON()
    {
        mapInformationList = new List<Map>();
        TextAsset mapText = Resources.Load<TextAsset>("Json/MapDefinition");
        if (mapText != null)
        {
            JSONObject mapJSON = new JSONObject(mapText.text);
            foreach (JSONObject mapInformation in mapJSON.list)
            {
                Map map = new Map
                {
                    MapId = (int)mapInformation["mapId"].n,
                    MapPicMappingId = mapInformation["mapPicMappingId"].str,
                    MapName = mapInformation["mapName"].str,
                    IsShowMapName = (bool)mapInformation["isShowMapName"].b
                };
                mapInformationList.Add(map);
            }
        }
    }
    #endregion

    #region 查詢場景名
    /*
     * 場景名是否在MapDefinition有定義    IsInMapDefinition(string SceneName)
     * 如果存在會返回在mapInformationList找到的index，若沒找到返回-1
     */
    public static int IsInMapDefinition(string SceneName)
    {
        int index = -1;
        int returnIndex = -1;
        foreach (Map map in mapInformationList)
        {
            index++;
            if (map.MapPicMappingId.Equals(SceneName))
            {
                returnIndex = index;
                break;
            }
        }
        return returnIndex;
    }
    #endregion

    #region 初始化道具欄
    private void initGetItem()
    {
        getItemNumList = new HashSet<int>();
        if (gameInitGetItemList != null && gameInitGetItemList.Count != 0)//當有設定初始化物品時
        {
            foreach (int initItemNum in gameInitGetItemList)
            {
                getItem(initItemNum);
            }
        }
    }
    #endregion

    #region 獲得道具    getItem(int itemNum)
    public static void getItem(int itemNum)
    {
        //在編號比定義檔小才會加入
        if (itemNum < itemInformationList.Count) getItemNumList.Add(itemNum);
        else Debug.Log("獲得道具失敗，道具編號超出范圍: " + itemInformationList.Count);
    }
    #endregion

    #region 失去道具    lostItem(int itemNum)
    public static void lostItem(int itemNum)
    {
        //物品號存在才會刪掉
        if (getItemNumList.Contains(itemNum)) getItemNumList.Remove(itemNum);
        else Debug.Log("失去道具失敗，找不到道具編號: " + itemNum);
    }
    #endregion

    #region 打印現有道具至Log  chackItem()
    public static void chackItem()
    {
        foreach (int itemNum in getItemNumList)
        {
            Debug.Log(
                itemInformationList[itemNum].ItemId + " " + 
                itemInformationList[itemNum].ItemName + " " + 
                itemInformationList[itemNum].ItemDescription + " " +
                itemInformationList[itemNum].ItemDescriptionbBasement + " " +
                itemInformationList[itemNum].UseItemameObject
            );
        }
    }
    #endregion

    #region 打印現有道具至Log  chackMap()
    public static void chackMap()
    {
        if (mapInformationList.Count == 0)
        {
            Debug.Log("mapInformationList.Count == 0");
        }
        foreach (Map map in mapInformationList)
        {
            Debug.Log(
                map.MapId + " " +
                map.MapPicMappingId + " " +
                map.MapName + " " +
                map.IsShowMapName
            );
        }
    }
    #endregion
}
