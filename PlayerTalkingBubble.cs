using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制主角頭上泡泡
public class PlayerTalkingBubble : MonoBehaviour
{
    //頭上對話泡泡物件
    private Animator talkBubbleAnimator;

    //先取得目前頭上泡泡動畫狀態的資訊
    AnimatorStateInfo currentState;

    private string shouldBeStatus;

    void Start()
    {
        //獲取子物件
        talkBubbleAnimator = this.transform.GetChild(2).gameObject.GetComponent<Animator>();
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
        if (currentState.IsName("empty") || currentState.IsName(status))
        {
            setStatus(status);
        }
        else
        {
            setStatus("empty");
        }
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
