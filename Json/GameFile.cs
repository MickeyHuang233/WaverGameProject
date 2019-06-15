using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameFile
{
    //遊戲存檔編號
    public int gameFileId;

    //存檔時玩家所在的地圖編號
    public int mapId;

    //存檔時玩家所在的X軸信息
    public float playerPositionX;

    //存檔時玩家所在的Y軸信息
    public float playerPositionY;

    //存檔時玩家所在的Z軸信息
    public float playerPositionZ;

    //存檔時玩家正在進行的劇情編號
    public int plotId;

    //存檔時的已遊玩時間_秒
    public int gameTimeSecond;

    //存檔時的已遊玩時間_分
    public int gameTimeMinute;

    //存檔時的系統時間
    public long saveTime;

    //已獲得道具
    public List<int> getItems;
    /*
    //是否完成各分支劇情
    public List<bool> branchSituation = new List<bool>();

    #region 初始化各分支劇情的List
    public void instaneBranchSituation(int branchSituationsize)
    {
        for (int i = 1; i <= branchSituationsize; i++) branchSituation.Add(false);
    }
    #endregion
    #region 修改某個分支劇情的List
    public void setBranchSituation (int index, bool isFinish)
    {
        branchSituation[index] = isFinish;
    }
    #endregion
    */
}
