using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlyPictureScene : MonoBehaviour
{
    [Header("下一個顯示的場景名")]
    public string startScene;

    [Header("到下一個場景的應休息時間")]
    [Range(0F, 10F)]
    public float restTime;

    //場景漸變物件的動畫組件
    public static Animator sceneGradientAnimator;

    //切換菜單tag已休息時間
    private float restTimer = 0F;

    // Start is called before the first frame update
    void Start()
    {
        //獲取場景漸變物件的動畫組件
        sceneGradientAnimator = GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Animator>();

        //初始化完成後才淡入場景
        sceneGradientAnimator.SetBool("changeScene_On", false);
        sceneGradientAnimator.SetBool("changeScene_Off", true);
    }

    // Update is called once per frame
    void Update()
    {
        //等待過了設定時間後淡出場景，並轉換至下一個場景
        restTimer += Time.deltaTime;
        if (restTimer >= restTime)
        {
            sceneGradientAnimator.SetBool("changeScene_On", true);
            sceneGradientAnimator.SetBool("changeScene_Off", false);
            if (restTimer >= restTime + 1F)
            {
                SceneManager.LoadScene(startScene);//新的加找場景方法
            }
        }
    }
}
