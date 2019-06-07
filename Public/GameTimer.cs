using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    //已遊戲時間，秒
    public static int second;

    //已遊戲時間，分
    public static int minute;

    //倒數秒數
    public static int nactivesecond;

    #region Start()
    void Start()
    {
        //每秒執行一次
        InvokeRepeating("positiveTiming", 1F, 1F);
        InvokeRepeating("nagativeTiming", 1F, 1F);
    }
    #endregion

    #region 正向計時
    void positiveTiming()
    {
        second++;
        if (second > 59)//分鐘進位
        {
            minute++;
            second = 0;
        }
    }
    #endregion

    #region 負向計時
    public void nagativeTiming()
    {
        if (nactivesecond >0) nactivesecond--;
    }
    #endregion
}
