using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 UGUI的鍵盤控制
*/
public class ButtonSelectInitialize : MonoBehaviour
{
    [SerializeField] private UnityEvent onStart;

    void Start()
    {
        this.onStart.Invoke();
    }
}
