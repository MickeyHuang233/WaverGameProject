using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    掛上此元件的話，此物件在場景轉換不會被unity釋放掉
    掛載物件：
        DoNotDestroy --> 場景轉換不需要被釋放的物件
    實現功能：
        在場景轉換不會被unity釋放掉
*/
public class DoNotDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
