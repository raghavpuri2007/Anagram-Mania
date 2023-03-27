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
    private MainPlayScript mainScript;
    private TextMeshProUGUI nameObject;
    private TextMeshProUGUI costObject;
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
    void enoughMoney() {
        if(!disabled && MainPlayScript.currentScore >= int.Parse(cost)) {
            thisPowerUp.interactable = true;
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
            doublePoints();
        } else if(name == "Immunity") {
            immunity();
        } else if(name == "Free Letter") {
            freeLetter();
            StartCoroutine(disableBtn(10f));
        }
        //call the helper method for the specific ability ie. "time freeze"
        //disable the power up for 20 seconds
    }
    IEnumerator disableBtn(float duration) {
        thisPowerUp.interactable = false;
        disabled = true;
        yield return new WaitForSeconds(duration);
        disabled = false;
        thisPowerUp.interactable = true;
    }
    IEnumerator freezeTime() {
        mainScript.increaseScore(-1500);
        mainScript.timeFreeze = true;
        yield return new WaitForSeconds(10f);
        mainScript.timeFreeze = false;
    }

    void streakBooster() {
        mainScript.increaseScore(-1500);
        mainScript.streak += 6;
        ProgressBar.maximum +=6;
        ProgressBar.currentBonus += 1000;
    }

    void doublePoints() {

    }
    
    void immunity() {

    }

    void freeLetter() {
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            for(int j = 0; j < MainPlayScript.dashes.Length; j++) {
                Vector3 position = new Vector3(MainPlayScript.dashes[j].transform.position.x, MainPlayScript.dashes[j].transform.position.y+150, 0);
                if(MainPlayScript.letters[i].transform.position == position) {
                    j = MainPlayScript.dashes.Length;
                } else {
                    if(j == MainPlayScript.dashes.Length-1) {
                        //found a letter not moved
                        int index = mainScript.currentWord.IndexOf(mainScript.scrambledWord[i]);
                        MainPlayScript.letters[i].transform.position = new Vector3(MainPlayScript.dashes[index].transform.position.x, MainPlayScript.dashes[index].transform.position.y + 150, 0);;
                        
                        //getting out of loops
                        i = MainPlayScript.letters.Length;
                        break;
                    }
                }
            }
        }
    }
    
}
