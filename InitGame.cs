using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//在遊戲初始化場景使用，加載完共用的元件後馬上轉至下一個場景
public class InitGame : MonoBehaviour
{
    [Header("初始化完成後要轉入的場景")]
    public string startScene;

    [Header("初始化完成後要轉入的場景傳送位置編號")]
    public string startPositionnName;

    void Start()
    {
        SceneManager.LoadScene(startScene);
    }
}
