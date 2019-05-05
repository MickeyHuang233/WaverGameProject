using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//掛上此元件的話，此物件在場景轉換不會被unity釋放掉
public class DoNotDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
