using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
public class RandomShowText : MonoBehaviour
{
    [Header("是否要顯示文字")]
    public bool canShowText = true;

    //要顯示的文字信息
    public string[] showTexts;

    [Header("建立文字串的數量")]
    [Range(0F, 50F)]
    public int createTextsNum = 5;

    [Header("使用的文字預載體")]
    public GameObject showTextFolder;

    //使用的文字顯示物件
    private GameObject showTextObject;

    //radomText的父物件
    private Transform showTextsObject;

    [Header("文字出現范圍_XMax")]
    public float radomXMax;

    [Header("文字出現范圍_XMin")]
    public float radomXMin;

    [Header("文字出現范圍_YMax")]
    public float radomYMax;

    [Header("文字出現范圍_Min")]
    public float radomYMin;

    [Header("文字顯示顏色")]
    public Color textColor = new Color(0F, 25F, 27F, 100F);

    [Header("文字描邊顯示顏色")]
    public Color textOutlineColor = Color.white;

    #region Start()
    void Start()
    {
        //根據玩家Y軸計算Z軸的值
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //獲得使用的文字顯示物件
        showTextObject = showTextFolder.transform.GetChild(0).gameObject;
        //莪得radomText的父物件
        showTextsObject = transform.GetChild(0).gameObject.transform;
    }
    #endregion

    #region Update()
    void Update()
    {
        if (canShowText && showTextsObject.childCount < createTextsNum) createRandomText(showTextsObject);
    }
    #endregion

    #region 建立顯示文字的物件   createRandomText(Transform showTextsObject)
    private void createRandomText(Transform showTextsObject)
    {
        //文字信息顯示
        string showText = showTexts[Random.Range(0, showTexts.Length)];
        showTextObject.GetComponent<Text>().text = showText;
        //文字顏色
        showTextObject.GetComponent<Text>().color = textColor;
        showTextObject.GetComponent<Outline>().effectColor = textOutlineColor;

        //生成的位置信息
        float radomX = Random.Range(radomXMin, radomXMax) + transform.position.x;
        float radomY = Random.Range(radomYMin, radomYMax) + transform.position.y;
        GameObject createTextObject = Instantiate(showTextFolder, new Vector3(radomX, radomY, 0), Quaternion.identity) as GameObject;
        createTextObject.transform.SetParent(showTextsObject);//將新建出來的radomText放到showTextsObject物件底下
        createTextObject.transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion

    #region 開炲顯示文字  openRandomShowText()
    public void openRandomShowText()
    {
        Debug.Log("canShowText = true");
        canShowText = true;
    }
    #endregion

    #region 取消顯示文字  closeRandomShowText()
    public void closeRandomShowText()
    {
        canShowText = false;
    }
    #endregion
}
