using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTagExit : MonoBehaviour
{
    //離開遊戲菜單文字物件
    GameObject exitPage;

    //離開遊戲菜單文字的動畫信息
    private Animator exitPageAanimator;

    //指標物件
    GameObject itemIndex;

    //當前位置編號
    private int tagIndex = 1;

    [Header("防按鍵鬼畜的休息時間")]
    [Range(0F, 5F)]
    public float restTime = 0.5F;

    //切換菜單tag已休息時間
    private float restTimer = 5F;

    //等待時間是否超過應等待時間
    private bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    #region 已休息時間歸零
    private void returnRestTimer()
    {
        restTimer = 0F;
    }
    #endregion

    #region Start()
    void Start()
    {
        exitPage = GameObject.Find("ExitPage");
        exitPageAanimator = exitPage.GetComponent<Animator>();
        itemIndex = this.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
    }
    #endregion

    #region Update()
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        float submit = Input.GetAxisRaw("Submit");//檢測z鍵
        restTimer += Time.deltaTime;
        if (overRestTime && PlayerItemMenu.openDetailMenu == 3)
        {
            if (v < 0 && tagIndex == 1)
            {
                Vector3 choicePosition_02 = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.position;
                itemIndex.transform.position = new Vector3(choicePosition_02.x - 0.25F, choicePosition_02.y, choicePosition_02.z);
                tagIndex++;
            } 
            else if (v > 0 && tagIndex == 2)
            {
                Vector3 choicePosition_01 = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.position;
                itemIndex.transform.position = new Vector3(choicePosition_01.x - 0.25F, choicePosition_01.y, choicePosition_01.z);
                tagIndex--;
            }
            if (submit > 0)
            {
                returnRestTimer();
                switch (tagIndex)
                {
                    case 1://是的
                        doExitSubmit();
                        break;
                    case 2://取消
                        doExitCancel();
                        break;
                }
            }
        }
    }
    #endregion

    # region 打開離開遊戲菜單
    private void showExitPage()
    {
        exitPageAanimator.SetBool("openDetilMenu", true);
        exitPageAanimator.SetBool("closeDetilMenu", false);
    }
    #endregion

    #region 關閉離開遊戲菜單
    private void hideExitPage()
    {
        exitPageAanimator.SetBool("openDetilMenu", false);
        exitPageAanimator.SetBool("closeDetilMenu", true);
    }
    #endregion

    #region 確認離開遊戲
    private void doExitSubmit()
    {
            Debug.Log("doExitSubmit");
            Application.Quit();//關閉遊戲
    }
    #endregion

    #region 取消離開遊戲
    private void doExitCancel()
    {
            Debug.Log("doExitCancel");
    }
    #endregion
}
