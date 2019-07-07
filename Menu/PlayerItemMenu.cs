using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    玩家菜單欄
    掛載物件：
        PlayerItemMenu --> Menu
            MenuTag
            Page
    實現功能：
        1. 控制菜單的層級顯示
*/
public class PlayerItemMenu : MonoBehaviour
{
    //玩家物件
    private GameObject playerObject;

    //先取得目前動畫狀態的資訊
    AnimatorStateInfo currentState;

    //菜單的動畫信息
    private Animator animator;

    /*
     * 菜單的Tag物件
     * 0 --> MenuTag_Item
     * 1 --> MenuTag_Save
     * 2 --> MenuTag_Exit
     */
    private List<GameObject> MenuTagsObjects = new List<GameObject>();

    /*
     *菜單Page物件
     * 0 --> MenuTag_Item
     * 1 --> MenuTag_Save
     * 2 --> MenuTag_Exit
     */
    private List<GameObject> MenuPageObjects = new List<GameObject>();

    /*
     * 打開二級菜單類型
     * -1 --> 未打開一級菜單
     * 0 --> 打開一級菜單，未打開二級菜單
     * 1 --> Item
     * 2 --> Save
     * 3 --> Exit
     */
    public static int openDetailMenu = -1;

    //當前位置編號
    private int tagIndex;

    //最大位置編號
    private int tagIndexMax = 3;

    [Header("切換菜單tag應休息時間")]
    [Range(0F, 5F)]
    public float setRestTime = 0.5F;

    //菜單所有操作的應休息時間
    public static float restTime;

    //切換菜單tag已休息時間
    public static float restTimer = 5F;

    //等待時間是否超過應等待時間
    public static bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    #region 已休息時間歸零
    public static void returnRestTimer()
    {
        restTimer = 0F;
    }
    #endregion

    #region Start()
    void Start()
    {
        //將設定的應休息時間放至靜態變量，以方便調用
        restTime = setRestTime;
        animator = GetComponent<Animator>();
        //取得玩家物件
        playerObject = GameObject.Find("Player");
        //獲取菜單各page物件
        for (int i = 0; i < transform.GetChild(0).childCount; i++) MenuPageObjects.Add(transform.GetChild(1).GetChild(i).gameObject);
        //取得菜單的Tag物件
        for (int i = 0; i < transform.GetChild(0).childCount; i++)  MenuTagsObjects.Add(transform.GetChild(0).GetChild(i).gameObject);
        tagIndex = 1;//初始化當前位置編號
    }
    #endregion

    #region Update()
    void Update()
    {
        currentState = animator.GetCurrentAnimatorStateInfo(0);//取得當前菜單動畫狀態的hashCode
       setMenuStatus(currentState);//設置菜單狀態

        float v = Input.GetAxisRaw("Vertical");//檢測垂直移動
        restTimer += Time.deltaTime;
        if (openDetailMenu == 0)//當開啟一級菜單
        {
            if (Input.GetButtonDown("Vertical")) doFirstMenuMove(v);
            if (Input.GetButtonDown("Submit") && overRestTime) doOpenDetailMenu();
            if (Input.GetButtonDown("Cancel") && overRestTime) doCloseMenu();
        }
        if(openDetailMenu > 0)//當開啟任一個二級菜單
        {
            if (Input.GetButtonDown("Cancel")) doCloseDetailMenu();
        }
    }
    #endregion

    #region 一級菜單選擇器的移動  doFirstMenuMove(float v)
    private void doFirstMenuMove(float v)
    {
        if (v > 0 && tagIndex > 1) tagIndex--;//向上移動
        else if (v < 0 && tagIndex < tagIndexMax) tagIndex++;//向下移動
    }
    #endregion

    #region 打開一級菜單  doOpenMenu()
    private void doOpenMenu()
    {
        Debug.Log("doOpenMenu");
        GameObject.Find("talkColliderObject").transform.localScale = new Vector3(0F, 0F, 0F);
        openDetailMenu = 0;
        returnRestTimer();
    }
    #endregion

    #region 關閉一級菜單  doCloseMenu()
    private void doCloseMenu()
    {
        Debug.Log("doCloseMenu");
        GameObject.Find("talkColliderObject").transform.localScale = new Vector3(1F, 1F, 1F);
        openDetailMenu = -1;
        tagIndex = 1;
    }
    #endregion

    #region 打開二級菜單  doOpenDetailMenu()
    private void doOpenDetailMenu()
    {
        MenuPageObjects[tagIndex - 1].SendMessage("showPage");
        openDetailMenu = tagIndex;
    }
    #endregion

    #region 關閉二級菜單  doCloseDetailMenu()
    private void doCloseDetailMenu()
    {
        MenuPageObjects[tagIndex - 1].SendMessage("hidePage");
        openDetailMenu = 0;
    }
    #endregion

    #region 統一管理菜單各元素的動畫狀態
    private void setMenuStatus(AnimatorStateInfo menuNowStateHash)
    {
        {
            if (openDetailMenu == -1)//未打開菜單
            {
                //設置Menu狀態
                if (menuIsStatus("OpenDetailMenu", menuNowStateHash)) setMenuStatus(true, false);
                else setMenuStatus(false, false);

                //設置Tag狀態
                foreach (GameObject MenuTagsObject in MenuTagsObjects) setTagStatus(MenuTagsObject.GetComponent<Animator>(), false, false, false);

                //設置Page狀態
                foreach (GameObject MenuPageObject in MenuPageObjects) setPageStatus(MenuPageObject.GetComponent<Animator>(), false);
            }
            else if (openDetailMenu == 0)//打開一級菜單
            {
                //設置Menu狀態
                setMenuStatus(true, false);

                //設置Tag狀態
                for (int i=0; i< MenuTagsObjects.Count; i++)
                {
                    if(i == (tagIndex - 1)) setTagStatus(MenuTagsObjects[i].GetComponent<Animator>(), true, false, true);
                    else setTagStatus(MenuTagsObjects[i].GetComponent<Animator>(), true, false, false);
                }

                //設置Page狀態
                foreach (GameObject MenuPageObject in MenuPageObjects) setPageStatus(MenuPageObject.GetComponent<Animator>(), false);
            }
            else if (openDetailMenu > 0)//打開二級菜單
            {
                //設置Menu狀態
                if (menuIsStatus("CloseMenu", menuNowStateHash)) setMenuStatus(true, false);
                else setMenuStatus(true, true);

                //設置Tag狀態
                for (int i = 0; i < MenuTagsObjects.Count; i++)
                {
                    if (i == (tagIndex - 1)) setTagStatus(MenuTagsObjects[i].GetComponent<Animator>(), true, true, true);
                    else setTagStatus(MenuTagsObjects[i].GetComponent<Animator>(), true, true, false);
                }

                //設置Page狀態
                for (int i = 0; i < MenuPageObjects.Count; i++)
                {
                    if (i == (tagIndex - 1)) setPageStatus(MenuPageObjects[i].GetComponent<Animator>(), true);
                    else setPageStatus(MenuPageObjects[i].GetComponent<Animator>(), false);
                }
            }
        }
    }
    #endregion

    #region 設置菜單狀態
    private void setMenuStatus(bool openMenu, bool openDetilMenu)
    {
        animator.SetBool("openMenu", openMenu);
        animator.SetBool("closeMenu", !openMenu);

        animator.SetBool("openDetilMenu", openDetilMenu);
        animator.SetBool("closeDetilMenu", !openDetilMenu);
    }
    #endregion

    #region 設置第二層菜單內容狀態
    private void setPageStatus(Animator menuPageAnimator, bool openDetilMenu)
    {
        menuPageAnimator.SetBool("openDetilMenu", openDetilMenu);
        menuPageAnimator.SetBool("closeDetilMenu", !openDetilMenu);
    }
    #endregion

    #region 設置Tag狀態
    private void setTagStatus(Animator menuTagAnimator, bool openMenu, bool openDetilMenu, bool choseTag)
    {
        menuTagAnimator.SetBool("openMenu", openMenu);
        menuTagAnimator.SetBool("closeMenu", !openMenu);

        menuTagAnimator.SetBool("openDetilMenu", openDetilMenu);
        menuTagAnimator.SetBool("closeDetilMenu", !openDetilMenu);

        menuTagAnimator.SetBool("choseTag", choseTag);
        menuTagAnimator.SetBool("cancelChoseTag", !choseTag);
    }
    #endregion

    #region 判斷當前菜單的狀態
    private bool menuIsStatus(string status, AnimatorStateInfo nowStateHash)
    {
        bool b = false;
        switch (status)
        {
            case "CloseMenu":
                b = nowStateHash.IsName("Base Layer.CloseMenu");
                break;
            case "OpenFirstMenu":
                b = nowStateHash.IsName("Base Layer.OpenFirstMenu") || nowStateHash.IsName("Base Layer.Menu_01_L") || nowStateHash.IsName("Base Layer.Menu_03_L");
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
