using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
物件周圍的周圍光 
*/
public class ShadowMask : MonoBehaviour
{
    [Range(0.05f, 0.2f)]
    public float flicTime;

    [Range(0.02f, 0.09f)]
    public float addSize;

    float timer = 0;
    private bool bigger = true;
    public Transform target;

    #region Update()
    public void Update()
    {
        timer += Time.deltaTime;
        //超過等待時間，燈光閃一下
        if (timer > flicTime)
        {
            if (bigger) transform.localScale = new Vector3(transform.localScale.x + addSize, transform.localScale.y + addSize, transform.localScale.z);
            else transform.localScale = new Vector3(transform.localScale.x - addSize, transform.localScale.y - addSize, transform.localScale.z);
            timer = 0;
            bigger = !bigger;
        }
        if (target != null) transform.position = Vector3.MoveTowards(transform.position, target.position, 20 * Time.deltaTime);
    }
    #endregion

}
