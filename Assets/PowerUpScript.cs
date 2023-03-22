using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerUpScript : MonoBehaviour
{
    public string name;
    public string cost;
    public Color32 nameColor;

    private TextMeshProUGUI nameObject;
    private TextMeshProUGUI costObject;
    // Start is called before the first frame update
    void Start()
    {
        nameObject = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        costObject = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameObject.text = name;
        costObject.text = cost;
        //changes color of button background // but don't need it
        // GetComponent<Image>().color = color;

        nameObject.color = nameColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void purchased() {
        //go through each power up and check if name matches
        //call the helper method for the specific ability ie. "time freeze"
        //disable the power up for x time
    }
}
