using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    控制主角頭上泡泡
    掛載物件：
        PlayerTalkingBubble --> talkBubble(對話框泡泡物件必須放在玩家物件底下的第三個)
    實現功能：
        1. 根據遊戲需求調用，在玩家頭上顯示對話泡泡
    對話泡泡對照：
        TalkBubble_01 --> 
        TalkBubble_02 --> 
        TalkBubble_03 --> 
*/
public class PlayerTalkingBubble : MonoBehaviour
{
    //頭上對話泡泡物件
    private Animator talkBubbleAnimator;

    //先取得目前頭上泡泡動畫狀態的資訊
    AnimatorStateInfo currentState;

    //應該所處的狀態名稱
    private string shouldBeStatus;

    void Start()
    {
        //獲取子物件
        talkBubbleAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();
        shouldBeStatus = "empty";
    }
    
    void Update()
    {
        currentState = talkBubbleAnimator.GetCurrentAnimatorStateInfo(0);//獲取當前的動畫狀態
        changShouldBeStatus(shouldBeStatus);
    }

    //設定批次狀態機的狀態
    private void setStatus(string status)
    {
        talkBubbleAnimator.SetBool("empty", false);
        talkBubbleAnimator.SetBool("TalkBubble_01", false);
        talkBubbleAnimator.SetBool("TalkBubble_02", false);
        talkBubbleAnimator.SetBool("TalkBubble_03", false);
        if (!status.Equals("allClose"))//只開啟所設定的狀態，若設定allClose則狀態全關
        {
            talkBubbleAnimator.SetBool(status, true);
        }
    }

    //判斷當前狀態是否為當前應該的狀態
    private void changShouldBeStatus(string status)
    {
        if (currentState.IsName("empty") || currentState.IsName(status)) setStatus(status);
        else setStatus("empty");
    }

    private void showTalkBubble_01()
    {
        shouldBeStatus = "TalkBubble_01";
    }

    private void showTalkBubble_02()
    {
        shouldBeStatus = "TalkBubble_02";
    }

    private void showTalkBubble_03()
    {
        shouldBeStatus = "TalkBubble_03";
    }

    private void hideTalkBubble()
    {
        shouldBeStatus = "empty";
    }
}
