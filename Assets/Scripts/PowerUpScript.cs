using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerUpScript : MonoBehaviour
{
    //power up details
    public string name;
    public string cost;
    public Color32 nameColor;
    private MainPlayScript mainScript;
    //ui objects passed in from game editor
    private TextMeshProUGUI nameObject;
    private TextMeshProUGUI costObject;
    //power up that the script is attached to
    public Button thisPowerUp;
    private bool disabled;
    // Start is called before the first frame update
    void Start()
    {
        nameObject = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        costObject = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameObject.text = name;
        costObject.text = cost;
        mainScript = GameObject.Find("Canvas").GetComponent<MainPlayScript>();
        //changes color of button background // but don't need it
        // GetComponent<Image>().color = color;
        thisPowerUp.interactable = false;
        disabled = false;
        nameObject.color = nameColor;
    }

    // Update is called once per frame
    void Update()
    {
        enoughMoney();
    }
    //checks if the user has enough money (score) to purchase power up
    void enoughMoney() {
        //has enough money, make it avaiable
        if(!disabled && MainPlayScript.currentScore >= int.Parse(cost)) {
            thisPowerUp.interactable = true;
        //otherwise disable the power up
        } else {
            thisPowerUp.interactable = false;
        }
    }
        // POWER UP STUFF
    public void purchased() {
        //go through each power up and check if name matches
        if(name == "Time Freeze") {
            StartCoroutine(freezeTime());
            StartCoroutine(disableBtn(25f));
        } else if(name == "Streak Boost") {
            streakBooster();
            StartCoroutine(disableBtn(20f));
        } else if(name == "Double Points") {
            //not implemented
            doublePoints();
        } else if(name == "Immunity") {
            //not implemented
            immunity();
        } else if(name == "Free Letter") {
            freeLetter();
            StartCoroutine(disableBtn(10f));
        }
    }
    //coroutine to disable the power up for current duration after duration
    IEnumerator disableBtn(float duration) {
        //disabled
        thisPowerUp.interactable = false;
        disabled = true;
        yield return new WaitForSeconds(duration);
        disabled = false;
        //reenable
        thisPowerUp.interactable = true;
    }
    //freeze time power up
    IEnumerator freezeTime() {
        //purchase the power up
        mainScript.increaseScore(-int.Parse(cost));
        //send info to main script
        mainScript.timeFreeze = true;
        yield return new WaitForSeconds(10f);
        mainScript.timeFreeze = false;
    }
    //streak booster power up
    void streakBooster() {
        //purchase the power up
        mainScript.increaseScore(-int.Parse(cost));
        mainScript.streak += 6;
        ProgressBar.maximum +=6;
        ProgressBar.currentBonus += 1000;
    }


    //power ups not created yet
    void doublePoints() {

    }
    
    void immunity() {

    }

    //free letter power up implementation
    void freeLetter() {
        //purchase the power up
        float height = Screen.height;
        mainScript.increaseScore(-int.Parse(cost));
        //go through each letter and dash
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            for(int j = 0; j < MainPlayScript.dashes.Length; j++) {
                //position of each dash
                float yPos = MainPlayScript.dashes[i].transform.position.y + (height/12f);
                if(height < 1000) {
                    yPos = MainPlayScript.dashes[i].transform.position.y + (height/10f);
                }
                Vector3 position = new Vector3(MainPlayScript.dashes[j].transform.position.x, yPos, 0);
                //if letter is on this dash, move onto new dash
                if(MainPlayScript.letters[i].transform.position == position) {
                    j = MainPlayScript.dashes.Length;
                } else {
                    if(j == MainPlayScript.dashes.Length-1) {
                        //found a letter not moved
                        int index = mainScript.currentWord.IndexOf(mainScript.scrambledWord[i]);
                        float otherYPos = MainPlayScript.dashes[i].transform.position.y + (height/12f);
                        if(height < 1000) {
                            otherYPos = MainPlayScript.dashes[i].transform.position.y + (height/10f);
                        }
                        MainPlayScript.letters[i].transform.position = new Vector3(MainPlayScript.dashes[index].transform.position.x, otherYPos, 0);                        
                        //getting out of loops
                        i = MainPlayScript.letters.Length;
                        break;
                    }
                }
            }
        }
    }
    
}
