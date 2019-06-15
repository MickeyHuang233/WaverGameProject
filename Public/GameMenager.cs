using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在遊戲初始化場景使用，加載完共用的元件後馬上轉至下一個場景
public class GameMenager : MonoBehaviour
{
    //所有遊戲道具信息
    public static List<Item> itemInformationList;

    //所有遊戲場景信息
    public static List<Map> mapInformationList;

    //所有遊戲劇情信息
    public static List<Plot> plotInformationList;

    [Header("遊戲初始獲得道具")]
    public List<int> gameInitGetItemList;

    [Header("遊戲的劇情編號")]
    public static int gamePlotNumber = 1;

    //物品欄已獲得物品編號
    public static HashSet<int> getItemNumList;

    //遊戲存檔信息
    public static GameFiles gameFiles;

    #region Start()
    void Start()
    {
        ParseMapJSON();//讀取場景json定義檔
        ParseItemJSON();//讀取物品json定義檔
        ParsePlotJSON();//讀取劇情json定義檔
        //clearGameFile();//清空gameFile
        loadJsonToBean();//讀取存檔信息json檔
        initGetItem();//初始化獲得的道具
    }
    #endregion

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

    #region 讀取劇情json定義檔，放入bean  ParsePlotJSON()
    private void ParsePlotJSON()
    {
        plotInformationList = new List<Plot>();
        TextAsset plotText = Resources.Load<TextAsset>("Json/PlotDefinition");
        if (plotText != null)
        {
            JSONObject plotJSON = new JSONObject(plotText.text);
            foreach (JSONObject plotInformation in plotJSON.list)
            {
                Plot plot = new Plot
                {
                    PlotId = (int)plotInformation["plotId"].n,
                    IsAction = (bool)plotInformation["isAction"].b,
                    IsOrder = (bool)plotInformation["isOrder"].b,
                    CanOpenMenu = (bool)plotInformation["canOpenMenu"].b,
                    RestartPoint = (bool)plotInformation["restartPoint"].b,
                    IsOrisOnlyWalkingder = (bool)plotInformation["isOnlyWalking"].b,
                    SituactionTarget = plotInformation["situactionTarget"].str
                };
                plotInformationList.Add(plot);
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

    #region 打印所有地圖信息至Log  chackAllMap()
    private void chackAllMap()
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

    #region 打印所有地圖信息至Log  chackAllPlot()
    private void chackAllPlot()
    {
        if (plotInformationList.Count == 0)
        {
            Debug.Log("mapInformationList.Count == 0");
        }
        foreach (Plot plot in plotInformationList)
        {
            Debug.Log(
                plot.PlotId + " " +
                plot.IsAction + " " +
                plot.IsOrder + " " +
                plot.CanOpenMenu + " " +
                plot.RestartPoint + " " +
                plot.IsOrisOnlyWalkingder + " " +
                plot.SituactionTarget
            );
        }
    }
    #endregion

    #region 找相應劇情編號的劇情信息，找不到返回null  findPlot(int plotId)
    public static Plot findPlotById(int targetPlotId)
    {
        Plot returnPlot = null;
        int index = -1;
        foreach (Plot plot in plotInformationList)
        {
            index++;
            if (plot.PlotId == targetPlotId)
            {
                returnPlot = plotInformationList[index];
                break;
            }
        }
        return returnPlot;
    }
    #endregion

    #region 將當前遊戲信息放入GameFile類  saveToBean(int gameFileId)
    public static GameFile saveToBean(int gameFileId)
    {
        //將已獲得道具Set轉成List
        List<int> getItems = new List<int>();
        foreach (int fetItemNum in getItemNumList) getItems.Add(fetItemNum);
        //儲存至Bean
        GameFile gameFile = new GameFile
        {
            gameFileId = gameFileId,//遊戲存檔編號
            mapId = GameMenager.IsInMapDefinition(InitSceneManagment.targetSceneName),//存檔時玩家所在的地圖編號
            playerPositionX = PlayerMovement.playerObject.transform.position.x,//存檔時玩家所在的X軸信息
            playerPositionY = PlayerMovement.playerObject.transform.position.y,//存檔時玩家所在的Y軸信息
            playerPositionZ = PlayerMovement.playerObject.transform.position.z,//存檔時玩家所在的Z軸信息
            plotId = GameMenager.gamePlotNumber,//存檔時玩家正在進行的劇情編號
            gameTimeSecond = GameTimer.second,//存檔時的已遊玩時間_秒
            gameTimeMinute = GameTimer.minute,//存檔時的已遊玩時間_分
            getItems = getItems,//已獲得道具
            saveTime = System.Convert.ToInt64((System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds)//存檔時的系統時間
            // TODO 是否完成各分支劇情，存檔
        };
        return gameFile;
    }
    #endregion

    #region 讀取存檔信息json檔 loadJsonToBean()
    public static void loadJsonToBean()
    {
        Debug.Log(PlayerPrefs.GetString("gameFile"));
        if (PlayerPrefs.GetString("gameFile") != null && (!PlayerPrefs.GetString("gameFile").Equals("")) && GameMenager.gameFiles == null && GameMenager.gameFiles.gameFiles.Count == 0)//有存過檔
        {
            GameMenager.gameFiles = JsonUtility.FromJson<GameFiles>(PlayerPrefs.GetString("gameFile"));
        }
        else//沒存過檔
        {
            GameMenager.gameFiles = new GameFiles();
            string saveString = JsonUtility.ToJson(GameMenager.gameFiles);//將gameFiles轉換成json格式的字串
            PlayerPrefs.SetString("gameFile", saveString);
        }
    }
    #endregion

    #region 將Bean轉為json並存到硬中
    public static void saveToJsonFile(int gameFileId)
    {
        GameFile gameFile = saveToBean(gameFileId);//先取存檔的json檔
        GameMenager.gameFiles.gameFiles[gameFileId] = gameFile;//覆蓋選中的檔案編號
        string saveString = JsonUtility.ToJson(GameMenager.gameFiles);//將gameFiles轉換成json格式的字串
        PlayerPrefs.SetString("gameFile", saveString);
    }
    #endregion

    #region 清空gameFile
    public static void clearGameFile()
    {
        PlayerPrefs.SetString("gameFile", "");
    }
    #endregion
}
