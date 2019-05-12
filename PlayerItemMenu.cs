using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家物品菜單欄
public class PlayerItemMenu : MonoBehaviour
{

    //菜單的動畫信息
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
    }

    void Update()
    {

    }

    //打開菜單
    private void doOpenMenu()
    {
        this.animator.SetBool("openMenu", true);
        this.animator.SetBool("closeMenu", false);
    }

    //關閉菜單
    private void doCloseMenu()
    {
        this.animator.SetBool("openMenu", false);
        this.animator.SetBool("closeMenu", true);
    }
}
