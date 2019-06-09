using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFile
{
    //遊戲存檔編號
    private int gameFileId;
    public int GameFileId { get; set; }
    
    //存檔時玩家所在的地圖編號
    private int mapId;
    public int MapId { get; set; }

    //存檔時玩家所在的X軸信息
    private float playerPositionX;
    public float PlayerPositionX { get; set; }

    //存檔時玩家所在的Y軸信息
    private float playerPositionY;
    public float PlayerPositionY { get; set; }

    //存檔時玩家所在的Z軸信息
    private float playerPositionZ;
    public float PlayerPositionZ { get; set; }

    //存檔時玩家正在進行的劇情編號
    private int plotId;
    public int PlotId { get; set; }

    //存檔時的已遊玩時間_秒
    private int gameTimeSecond;
    public int GameTimeSecond { get; set; }

    //存檔時的已遊玩時間_分
    private int gameTimeMinute;
    public int GameTimeMinute { get; set; }

    //是否完成各分支劇情
    private List<bool> branchSituation = new List<bool>();
    public List<bool> BranchSituation { get; set; }

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
}
