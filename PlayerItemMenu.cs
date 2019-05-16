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

    void Start()
    {
        animator = GetComponent<Animator>();
        //設定菜單動畫狀態
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
        //取得菜單的Tag物件及動畫組件
        for (int i = 0; i < this.transform.GetChildCount(); i++)
        {
            GameObject t = this.transform.GetChild(i).gameObject;
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

    void Update()
    {

    }

    //打開菜單
    private void doOpenMenu()
    {
        this.animator.SetBool("openMenu", true);
        this.animator.SetBool("closeMenu", false);
        for (int i = 0; i < this.transform.GetChildCount(); i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", true);
            MenuTagsAnimator[i].SetBool("closeMenu", false);
        }
    }

    //關閉菜單
    private void doCloseMenu()
    {
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
        for (int i = 0; i < this.transform.GetChildCount(); i++)
        {
            MenuTagsAnimator[i].SetBool("openMenu", false);
            MenuTagsAnimator[i].SetBool("closeMenu", true);
        }
    }
}
