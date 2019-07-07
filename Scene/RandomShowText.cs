using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
public class RandomShowText : MonoBehaviour
{
    //要顯示的文字信息
    public string[] showTexts;

    [Header("建立文字串的數量")]
    [Range(0F, 30F)]
    public int createTextsNum = 5;

    [Header("使用的文字預載體")]
    public GameObject showTextFolder;

    //使用的文字顯示物件
    private GameObject showTextObject;

    //radomText的父物件
    private Transform showTextsObject;

    [Header("文字出現范圍_XMax")]
    [Range(-5F, 5F)]
    public float radomXMax;

    [Header("文字出現范圍_XMin")]
    [Range(-5F, 5F)]
    public float radomXMin;

    [Header("文字出現范圍_YMax")]
    [Range(-5F, 5F)]
    public float radomYMax;

    [Header("文字出現范圍_Min")]
    [Range(-5F, 5F)]
    public float radomYMin;

    [Header("文字顯示顏色")]
    public Color textColor = new Color(0F, 25F, 27F, 100F);

    [Header("文字描邊顯示顏色")]
    public Color textOutlineColor = Color.white;

    void Start()
    {
        //根據玩家Y軸計算Z軸的值
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //獲得使用的文字顯示物件
        showTextObject = showTextFolder.transform.GetChild(0).gameObject;
        //莪得radomText的父物件
        showTextsObject = transform.GetChild(0).gameObject.transform;
        //建立文字信息
        for (int i = 1;i<= createTextsNum;i++) createRandomText(showTextsObject);
    }

    void Update()
    {
        if (showTextsObject.childCount < createTextsNum) createRandomText(showTextsObject);
    }

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
    }
}
