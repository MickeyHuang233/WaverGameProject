using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
物件周圍的周圍光 
*/
public class ShadowMask : MonoBehaviour
{
    [Header("手電筒類別")]
    public string ShadowMaskType;

    [Header("等待時間范圍")]
    [Range(0.5F, 10F)]
    public float restTime = 5F;

    [Header("增加或減少DarkLayout的大小范圍")]
    [Range(0.00F, 0.1F)]
    public float addSize;

    [Header("調整X軸")]
    [Range(-1.0F, 1.0F)]
    public float ModifyX = 0F;

    [Header("調整Y軸")]
    [Range(-1.0F, 1.0F)]
    public float ModifyY = 0F;

    public float playerModify = 0.5F;

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

    [Header("目標跟隨物體是否是菜單")]
    public bool isMenu = false;

    //手電筒大小，暫存
    private float scaleX;
    private float scaleY;

    //手電絧偏移量，暫存
    private float modifyX;
    private float modifyY;

    #region Start()
    public void Start()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        modifyX = ModifyX;
        modifyY = ModifyY;
        if (isMenu) transform.localScale = new Vector3(0, 0, 0);
    }
    #endregion

    #region Update()
    public void Update()
    {
        if (!ShadowMaskType.Equals(InitSceneManagment.mapLayoutType))
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            transform.localScale = new Vector3(scaleX, scaleY, 0);
            RestTimer += Time.deltaTime;
            if (!isMenu || PlayerItemMenu.openDetailMenu > 0) darkLayoutShine();//超過等待時間，燈光閃一下
            if (isMenu) menuTriggerDarkLayout();
            if (isPlayer) playerTriggerDarkLayout();
            darkLayoutMove();//DarkLayout的移動
        }
    }
    #endregion

    #region 菜單顯示或隱藏DarkLayout   menuTriggerDarkLayout()
    private void menuTriggerDarkLayout()
    {
        float timer = Time.deltaTime * 6;
        //當玩家打開一級菜單時另外顯示光源
        if (PlayerItemMenu.openDetailMenu < 0)//沒打開菜單
        {
            transform.localScale = new Vector3(0, 0, 0);
            ModifyX = (ModifyX <= 0.005) ? 0 : Mathf.Lerp(ModifyX, 0, timer);
            ModifyY = (ModifyY <= 0.005) ? 0 : Mathf.Lerp(ModifyY, 0, timer);
        }
        else if (PlayerItemMenu.openDetailMenu == 0)//打開一級菜單
        {
            transform.localScale = new Vector3(scaleX / 2, scaleY / 2, 0);
            ModifyX = Mathf.Lerp(ModifyX, modifyX / 2, timer);
            ModifyY = Mathf.Lerp(ModifyY, modifyY / 2, timer);
        }
        else//打開二級菜單
        {
            transform.localScale = new Vector3(scaleX, scaleY, 0);
            ModifyX = (ModifyX >= modifyX * 0.99) ? modifyX : Mathf.Lerp(ModifyX, modifyX, timer);
            ModifyY = (ModifyY >= modifyY * 0.99) ? modifyY : Mathf.Lerp(ModifyY, modifyY, timer);
        }
    }
    #endregion

    #region 菜單顯示或隱藏時玩家的DarkLayout   playerTriggerDarkLayout()
    private void playerTriggerDarkLayout()
    {
        float timer = Time.deltaTime * 6;
        //當玩家打開一級菜單時另外顯示光源
        if (PlayerItemMenu.openDetailMenu < 0)//沒打開菜單
        {
            //transform.localScale = new Vector3(scaleX, scaleY, 0);
            playerModify = 0.5F;
        }
        else//打開菜單
        {
            //transform.localScale = new Vector3(scaleX / 2, scaleY / 2, 0);
            playerModify = 0.01F;
        }
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
                float v = (PlayerMovement.moveDirction.x > 0) ? playerModify : (PlayerMovement.moveDirction.x == 0) ? 0F : -playerModify;
                float h = (PlayerMovement.moveDirction.y > 0) ? playerModify : (PlayerMovement.moveDirction.y == 0) ? 0F : -playerModify;
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
                    Vector3 targetPosition = new Vector3(targetPositionX + ModifyX, targetPositionY + ModifyY, -1);
                    transform.GetChild(i).position = Vector3.MoveTowards(transform.GetChild(i).position, targetPosition, darkLayoutMoveSpeed * Time.deltaTime / (i + 1));
                }
            }
            else
            {
                Vector3 targetPosition = new Vector3(targetObjectPositon.position.x + ModifyX, targetObjectPositon.position.y + ModifyY, -1);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, darkLayoutMoveSpeed * Time.deltaTime);
            }
        }
    }
    #endregion

}
