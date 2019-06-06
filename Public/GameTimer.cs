using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    //已遊戲時間，秒
    public static int second;

    //已遊戲時間，分
    public static int minute;

    #region Start()
    void Start()
    {
        InvokeRepeating("timing", 1F, 1F);
    }
    #endregion

    void timing()
    {
        second++;
        if (second > 59)
        {
            minute++;
            second = 0;
        }
    }
}
