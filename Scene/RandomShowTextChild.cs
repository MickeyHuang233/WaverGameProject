using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShowTextChild : MonoBehaviour
{
    //動畫信息
    private Animator animator;

    //隨機最小秒數
    private float radomMinTime = 5F;

    //隨機最大秒數
    private float radomMaxTime = 20F;

    void Start()
    {
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        StartCoroutine(showTextChild());//協程_顯示文字
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.ShowText") || animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.HidingText"))
        {
            StartCoroutine(hideTextChild());//協程_隱藏並銷毀文字
        }

    }

    #region 協程_顯示文字
    IEnumerator showTextChild()
    {
        float radomShowWaitTime = Random.Range(radomMinTime, radomMaxTime);
        yield return new WaitForSeconds(radomShowWaitTime);
        setPageStatus(true);
    }
    #endregion

    #region 協程_隱藏並銷毀文字
    IEnumerator hideTextChild()
    {
        float radomHideWaitTime = Random.Range(radomMinTime, radomMaxTime * 2);
        yield return new WaitForSeconds(radomHideWaitTime);
        setPageStatus(false);
        yield return new WaitForSeconds(5F);
        Destroy(gameObject);
    }
    #endregion

    #region 設置文字動畫狀態
    private void setPageStatus(bool showText)
    {
        animator.SetBool("showText", showText);
        animator.SetBool("hideText", !showText);
    }
    #endregion

    #region 判斷當前菜單的狀態
    private bool isStatus(string status, AnimatorStateInfo nowStateHash)
    {
        bool b = false;
        switch (status)
        {
            case "CloseMenu":
                b = nowStateHash.IsName("Base Layer.CloseMenu");
                break;
            case "OpenFirstMenu":
                b = nowStateHash.IsName("Base Layer.ShowText") || nowStateHash.IsName("Base Layer.HidingText") || nowStateHash.IsName("Base Layer.Menu_03_L");
                break;
            case "OpenDetailMenu":
                b = nowStateHash.IsName("Base Layer.OpenDetailMenu") || nowStateHash.IsName("Base Layer.Menu_04_L") || nowStateHash.IsName("Base Layer.Menu_06_L");
                break;
            default:
                Debug.Log(status + "找不到相對應的狀態，可能輸入錯誤");
                break;
        }
        return b;
    }
    #endregion
}
