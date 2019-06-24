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

    //切換菜單tag已休息時間
    private float restTimer = 0F;

    //等待時間是否超過應等待時間
    public bool overRestTime
    {
        get
        {
            return (restTimer < restTime) ? false : true;//休息時間還沒到就返回false
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        restTimer += Time.deltaTime;
        if (overRestTime) SceneManager.LoadScene(startScene);//新的加找場景方法
    }
}
