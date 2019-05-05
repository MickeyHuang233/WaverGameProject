using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//控制遊戲主頁面
public class TitleSceneControl : MonoBehaviour
{
    public string startScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doGameStart()
    {
        //Application.LoadLevel("Scene_1000");//舊方法，可透過場景名或場景編號加載場景
        SceneManager.LoadScene(startScene);//新的加找場景方法
    }
}
