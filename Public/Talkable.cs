using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/*
    控制對話相關開關及判斷
    掛載物件：
        Talkable --> SpeakManager
    實現功能：
        1. 判斷是否正在對話中，方便其他物件獲取及判斷
*/
public class Talkable : MonoBehaviour
{
    //get SpeakManager compoment，對話管理器
    public static Flowchart flowchartManager;
    
    void Start()
    {
        flowchartManager = GameObject.Find("SpeakManager").GetComponent<Flowchart>();
    }

    //判斷當前是否正在對話
    public static bool isTalking
    {
        get
        {
            return flowchartManager.GetBooleanVariable("isTalking");
        }
    }
}
