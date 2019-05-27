using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    遊戲初始畫面的Button方法
    掛載物件：
        TitleSceneControl --> 
    實現功能：
        1. 遊戲開始後加載初始化場景 // TODO 畫面需要淡出
        2. // TODO 遊戲讀取的操作
        3. // TODO 遊戲關閉的操作
*/
public class TitleSceneControl : MonoBehaviour
{
    //初始化場景名
    public string startScene;
    
    //遊戲開始
    public void doGameStart()
    {
        //Application.LoadLevel("Scene_1000");//舊方法，可透過場景名或場景編號加載場景
        SceneManager.LoadScene(startScene);//新的加找場景方法
    }
    
    //讀取遊戲
    public void doGameLoad()
    {
        
    }
    
    //關閉遊戲
    public void doGameExit()
    {
    
    }
}
