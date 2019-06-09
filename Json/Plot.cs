using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot
{
    //劇情編號
    private int plotId;
    public int PlotId { get; set; }

    //角色是否能隨意移動
    private bool isAction;
    public bool IsAction { get; set; }

    //劇情流程是否有順序之分
    private bool isOrder;
    public bool IsOrder { get; set; }

    //能否開菜單
    private bool canOpenMenu;
    public bool CanOpenMenu { get; set; }

    //是否是復活點
    private bool restartPoint;
    public bool RestartPoint { get; set; }
    
    //是否只能走路(也就是不能跑步)
    private bool IsOnlyWalking;
    public bool IsOrisOnlyWalkingder { get; set; }
    
    //主線任務內容，在菜單存檔頁面顯示的內容
    private string situactionTarget;
    public string SituactionTarget { get; set; }
    
}
