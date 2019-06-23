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

    [Header("DarkLayout移動速度")]
    [Range(0F, 50F)]
    public float darkLayoutMoveSpeed = 20F;

    //已等待時間
    float RestTimer = 0;

    //是否大於等待時間
    private bool isBiggerRestTime = true;

    [Header("目標跟隨物體")]
    public Transform targetObjectPositon;

    [Header("目標跟隨物體是否是玩家")]
    public bool isPlayer = false;

    #region Update()
    public void Update()
    {
        RestTimer += Time.deltaTime;
        darkLayoutShine();//超過等待時間，燈光閃一下
        darkLayoutMove();//DarkLayout的移動
    }
    #endregion

    #region 超過等待時間，燈光閃一下   darkLayoutShine()
    private void darkLayoutShine()
    {
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
    }
    #endregion

    #region DarkLayout的移動   darkLayoutMove()
    private void darkLayoutMove()
    {
        if (targetObjectPositon != null)
        {
            if (isPlayer)
            {
                float v = (PlayerMovement.moveDirction.x > 0) ? 0.5F : (PlayerMovement.moveDirction.x == 0) ? 0F : -0.5F;
                float h = (PlayerMovement.moveDirction.y > 0) ? 0.5F : (PlayerMovement.moveDirction.y == 0) ? 0F : -0.5F;
                //取得目標物件位置
                float targetPositionX = targetObjectPositon.position.x + v;
                float targetPositionY = targetObjectPositon.position.y + h;
                //設置迴圈次數
                int forCount = 0;
                switch (InitSceneManagment.mapLayoutType)
                {
                    case "1F":
                        forCount = 2;
                        break;
                    case "3F":
                        forCount = 2;
                        break;
                    case "B1":
                        forCount = 4;
                        break;
                }

                for (int i = 0; i < forCount; i++)
                {
                    Vector3 targetPosition = new Vector3(targetPositionX + modifyX, targetPositionY + modifyY, -1);
                    transform.GetChild(i).position = Vector3.MoveTowards(transform.GetChild(i).position, targetPosition, darkLayoutMoveSpeed * Time.deltaTime / (i + 1));
                }
            }
            else
            {
                Vector3 targetPosition = new Vector3(targetObjectPositon.position.x + modifyX, targetObjectPositon.position.y + modifyY, -1);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, darkLayoutMoveSpeed * Time.deltaTime);
            }
        }
    }
    #endregion

}
