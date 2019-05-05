﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用於判斷被調查物件進入觸發器及調查的信息發送
public class SurveyJudge : MonoBehaviour
{
    //Player物件
    private GameObject playerObject;

    //進入觸發器的物件
    static Dictionary<string, float> triggerNameMap;

    //取得需要被調查的物件
    private GameObject surveyObject;

    //觸發對話或調查的物件名
    string minDistanceObject;

    void Start()
    {
        //獲取父類的Player物件
        playerObject = this.transform.parent.gameObject;
        triggerNameMap = new Dictionary<string, float>();
    }
    
    void Update()
    {
        countAllTriggerDistance();
    }

    //判斷是否是可被調查的物件tag
    private bool isTag(Collider2D collision)
    {
        return collision.tag == "NPC" || collision.tag == "NPC_Item" || collision.tag == "Portal";
    }

    // 計算所有進入觸發器的物件與玩家的距離
    private void countAllTriggerDistance()
    {
        Vector2 playerPosition = playerObject.transform.position;
        List<string> keys = new List<string>(triggerNameMap.Keys);
        //計算每個進入觸發器的物件與玩家之間的距離
        foreach (var key in keys)
        {
            Vector2 npcPosition = GameObject.Find(key).transform.position;
            float distance = Vector2.Distance(playerPosition, npcPosition);
            triggerNameMap[key] = distance;
        }

        //當有多個物件進入觸發器時，比較出最接近玩家的物件
        if (keys.Count >= 1)
        {
            minDistanceObject = keys[0];
            for (int i = 0; i < keys.Count - 1; i++)
            {
                if (triggerNameMap[keys[i + 1]] < triggerNameMap[keys[i]])
                {
                    minDistanceObject = keys[i + 1];
                }
            }
            //取得需要被調查的物件
            surveyObject = GameObject.Find(minDistanceObject);
            Collider2D surveyObjectCollider = surveyObject.GetComponent<Collider2D>();
            //其他物件
            Collider2D otherObjectCollider;
            //只標注需要被調查的物件，其他物件都需要取消標注
            surveyObjectCollider.SendMessage("doHightLineOn");
            for (int i = 0; i < keys.Count ; i++)
            {
                if (!keys[i].Equals(minDistanceObject))
                {
                    GameObject.Find(keys[i]).GetComponent<Collider2D>().SendMessage("doHightLineOff");
                }
            }

            if (surveyObjectCollider.tag == "NPC")
            {
                playerObject.SendMessage("showTalkBubble_01");
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    // TODO SurveyJudge，與NPC對話按Z後的操作
                }
            }
            else if (surveyObjectCollider.tag == "NPC_Item")
            {
                playerObject.SendMessage("showTalkBubble_02");
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    // TODO SurveyJudge，調查物品按Z後的操作
                }
            }
            else if (surveyObjectCollider.tag == "Portal")
            {
                playerObject.SendMessage("showTalkBubble_03");
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    surveyObject.SendMessage("ChangeScene");
                    playerObject.SendMessage("hideTalkBubble");
                    triggerNameMap.Clear();
                }
            }

        }
    }

    //當物件進入觸發器時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTag(collision))
        {
            //將符合條件的物件加入至Map中
            triggerNameMap.Add(collision.transform.name, 0F);
        }
    }

    //當物件離開觸發器時
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isTag(collision))
        {
            playerObject.SendMessage("hideTalkBubble");
            triggerNameMap.Remove(collision.transform.name);//將符合條件的物件移除
            collision.SendMessage("doHightLineOff");
        }
    }
}
