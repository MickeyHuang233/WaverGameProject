using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
物件周圍的周圍光 
*/
public class ShadowMask : MonoBehaviour
{
    [Header("等待時間范圍")]
    [Range(0.5F, 10F)]
    public float restTime = 5F;

    [Header("對話完成或關閉菜單所需等待時間")]
    [Range(0.00F, 0.1F)]
    public float addSize;

    [Header("調整X軸")]
    [Range(-1.0F, 1.0F)]
    public float modifyX = 0F;

    [Header("調整Y軸")]
    [Range(-1.0F, 1.0F)]
    public float modifyY = 0F;

    //已等待時間
    float RestTimer = 0;

    //是否大於等待時間
    private bool isBiggerRestTime = true;

    [Header("目標跟隨物體")]
    public Transform target;

    #region Update()
    public void Update()
    {
        RestTimer += Time.deltaTime;

        //隨機産生應等待時間
        float randomRestTime = Random.Range(0F, restTime);
        //隨機産生應改變大小
        float randomAddSize = Random.Range(-addSize, addSize);

        //超過等待時間，燈光閃一下
        if (RestTimer > randomRestTime)
        {
            if (isBiggerRestTime) transform.localScale = new Vector3(transform.localScale.x + randomAddSize, transform.localScale.y + randomAddSize, transform.localScale.z);
            else transform.localScale = new Vector3(transform.localScale.x - randomAddSize, transform.localScale.y - randomAddSize, transform.localScale.z);
            RestTimer = 0;
            isBiggerRestTime = !isBiggerRestTime;
        }

        //燈光隨目標物件移動
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x + modifyX, target.transform.position.y + modifyY, -1);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20 * Time.deltaTime);
        }
    }
    #endregion

}
