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
    private MainPlayScript script;
    private TextMeshProUGUI nameObject;
    private TextMeshProUGUI costObject;
    // Start is called before the first frame update
    void Start()
    {
        nameObject = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        costObject = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameObject.text = name;
        costObject.text = cost;
        script = GameObject.Find("Canvas").GetComponent<MainPlayScript>();
        //changes color of button background // but don't need it
        // GetComponent<Image>().color = color;

        nameObject.color = nameColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        // POWER UP STUFF
    public void purchased() {
        //go through each power up and check if name matches
        if(name == "Time Freeze") {
            StartCoroutine(freezeTime());
        } else if(name == "Streak Booster") {
            streakBooster();
        } else if(name == "Double Points") {
            doublePoints();
        } else if(name == "Immunity") {
            immunity();
        } else if(name == "Free Letter") {
            freeLetter();
        }
        //call the helper method for the specific ability ie. "time freeze"
        //disable the power up for 20 seconds
    }

    IEnumerator freezeTime() {
        script.increaseScore(-1000);
        script.timeFreeze = true;
        yield return new WaitForSeconds(10f);
        script.timeFreeze = false;
    }

    void streakBooster() {

    }

    void doublePoints() {

    }
    
    void immunity() {

    }

    void freeLetter() {

    }
    
}
