using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    遊戲地圖信息
*/
public class Map
{
    //場景編號
    private int mapId;
    public int MapId { get; set; }

    //Unity場景名稱
    private string mapPicMappingId;
    public string MapPicMappingId { get; set; }

    //顯示的場景名稱
    private string mapName;
    public string MapName { get; set; }

    //是否顯示地圖名稱
    private bool isShowMapName;
    public bool IsShowMapName { get; set; }

}
