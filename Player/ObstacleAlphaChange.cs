using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAlphaChange : MonoBehaviour
{
    #region 當物件進入觸發器時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //將符合條件的物件加入至Map中
        if (collision.tag.Equals("ObstacleAlphaChange")) collision.transform.parent.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.8F);
    }
    #endregion

    #region 當物件離開觸發器時
    private void OnTriggerExit2D(Collider2D collision)
    {
        //將符合條件的物件加入至Map中
        if (collision.tag.Equals("ObstacleAlphaChange")) collision.transform.parent.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }
    #endregion
}
