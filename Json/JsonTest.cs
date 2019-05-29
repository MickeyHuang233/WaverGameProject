using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ParseJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ParseJSON()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Json/ItemDefinition");
        if (itemText != null)
        {
            JSONObject itemJSON = new JSONObject(itemText.text);
            foreach (JSONObject item in itemJSON.list)
            {
                int itemId = (int)item["itemId"].n;
                string itemName = item["itemName"].str;
                string description = item["description"].str;
                string gameObject = item["gameObject"].str;
                Debug.Log(itemId + " " + itemName + " " + description + " " + gameObject);
            }
        }
    }
}
