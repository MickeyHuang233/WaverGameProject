using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家物品菜單欄
public class PlayerItemMenu : MonoBehaviour
{
    //菜單的動畫信息
    private Animator animator;

    //菜單的Tag物件
    private List<GameObject> MenuTagsObject = new List<GameObject>();

    //菜單的Tag動畫組件
    private List<Animator> MenuTagsAnimator = new List<Animator>();

    [Header("菜單選項選擇器")]
    public GameObject MenuChoice;

    //是否打開第二級層菜單
    public static bool openDetailMenu = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        //設定菜單動畫狀態
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
        //取得菜單的Tag物件及動畫組件
        for (int i = 0; i < this.transform.GetChild(0).GetChildCount(); i++)
        {
            GameObject t = this.transform.GetChild(0).GetChild(i).gameObject;
            MenuTagsObject.Add(t);
            Animator a = t.GetComponent<Animator>();
            MenuTagsAnimator.Add(a);
            //設定菜單下Tag動畫狀態
            a.SetBool("choseTag", false);
            a.SetBool("cancelChoseTag", false);
            a.SetBool("openMenu", false);
            a.SetBool("closeMenu", false);
        }
    }

    //打開一級菜單
    private void doOpenMenu()
    {
        this.animator.SetBool("openMenu", true);
        this.animator.SetBool("closeMenu", false);
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", true);
            MenuTagsAnimator[i].SetBool("closeMenu", false);
            MenuTagsAnimator[i].SetBool("choseTag", false);
            MenuTagsAnimator[i].SetBool("cancelChoseTag", true);
            if (i == 0)
            {
                MenuTagsAnimator[i].SetBool("choseTag", true);
                MenuTagsAnimator[i].SetBool("cancelChoseTag", false);
            }
        }
        Vector3 tagItemPosition = MenuTagsObject[0].transform.position;
        //如果沒有菜單選擇物件就加載
        if (GameObject.Find("MenuChoice(Clone)") == null)
        {
            GameObject go = GameObject.Instantiate(MenuChoice, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.transform.SetParent(transform);//將新建出來的菜單選項選擇器放到自己底下
        }
    }

    //關閉一級菜單
    private void doCloseMenu()
    {
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
        for (int i = 0; i < MenuTagsObject.Count; i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", false);
            MenuTagsAnimator[i].SetBool("closeMenu", true);
            MenuTagsAnimator[i].SetBool("choseTag", false);
            MenuTagsAnimator[i].SetBool("cancelChoseTag", true);
        }
        //如果有菜單選擇物件就注除
        if (GameObject.Find("MenuChoice(Clone)") != null)
        {
            GameObject.Destroy(GameObject.Find("MenuChoice(Clone)"));
        }
    }

    //打開二級菜單
    private void doOpenDetailMenu()
    {
        this.animator.SetBool("openDetilMenu", true);
        this.animator.SetBool("closeDetilMenu", false);
        openDetailMenu = true;
    }

    //關閉二級菜單
    private void doCloseDetailMenu()
    {
        this.animator.SetBool("openDetilMenu", false);
        this.animator.SetBool("closeDetilMenu", true);
        openDetailMenu = false;
    }
}
