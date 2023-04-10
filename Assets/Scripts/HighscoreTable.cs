using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//Leaderboard class
public class HighscoreTable : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entry;
    private List<Transform> highScoreEntryTransformList;

    public void Update() {
        //escape key pressed, quit application
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    //method that runs when the program starts
    private void Awake() {
        entryContainer = transform.Find("HighScoreContainer");
        entry = entryContainer.Find("HighScoreEntry");
        entry.gameObject.SetActive(false);

        //CODE TO CLEAR LEADERBOARD
        // string jsonClear = JsonUtility.ToJson(new List<HighscoreEntry>());
        // PlayerPrefs.SetString("highscoreTable", jsonClear);
        // PlayerPrefs.Save();

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        if(jsonString == "") {
            Highscores highscoresTest = new Highscores { highScoreEntryList = new List<HighscoreEntry>()};
            string jsonTest = JsonUtility.ToJson(highscoresTest);
            PlayerPrefs.SetString("highscoreTable", jsonTest);
            PlayerPrefs.Save();
            jsonString = PlayerPrefs.GetString("highscoreTable");
        }
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highScoreEntryList = sortList(highscores);

        highScoreEntryTransformList = new List<Transform>();
        //for each entry in the list, display it
        foreach(HighscoreEntry highscore in highscores.highScoreEntryList) {
            CreateHighscoreEntryTransform(highscore, entryContainer, highScoreEntryTransformList);
        }
    }
    //method to display entry
    private void CreateHighscoreEntryTransform(HighscoreEntry highScoreEntry, Transform container, List<Transform> transformList) {
        //pixel difference between each entry
        float templateHeight = 95f;
        Transform entryTransfrom = Instantiate(entry, container);
        //make the entry viewable
        entryTransfrom.gameObject.SetActive(true);
        RectTransform entryRectTransform = entryTransfrom.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, entry.GetComponent<RectTransform>().anchoredPosition.y-(templateHeight * transformList.Count));

        int rank = transformList.Count+1;
        //get icon for first three positions
        Image icon =  entryTransfrom.Find("Icon").GetComponent<UnityEngine.UI.Image>();
        if(rank == 1) {
            //gold color for 1st place
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
        } else if(rank == 2) {
            //silver color for 2nd place
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
        } else if(rank == 3) {
            //bronze color for 3rd place
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
        }
        string rankString;
        //statement to make the first 3 positions look nice
        switch(rank) {
            //anything except 1,2,3
            default: 
                rankString = rank + "TH";
                icon.enabled  = false;
                break;
            //set 1st place to bronze
            case 1: 
                rankString = "1ST"; 
                icon.color = new Color32(217, 194, 21, 255);
                break;
            //set 2nd place to silver
            case 2: 
                rankString = "2ND"; 
                icon.color = new Color32(192, 192, 192, 255);
                break;
            //set 3rd place to bronze
            case 3: 
                rankString = "3RD"; 
                icon.color = new Color32(205, 127, 50, 255);
                break;
        }
        //grab the components of each entry
        entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().text = rankString;
        float score = highScoreEntry.score;
        entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
        entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().text = highScoreEntry.name;

        transformList.Add(entryTransfrom);
    }
    //method to add entry to leaderboard object
    public static void AddHighScoreEntry(float score, string name) {

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        //safety case if the leaderboard has not been created.
        if(jsonString == "") {
            Highscores highscoresTest = new Highscores { highScoreEntryList = new List<HighscoreEntry>()};
            string jsonTest = JsonUtility.ToJson(highscoresTest);
            PlayerPrefs.SetString("highscoreTable", jsonTest);
            PlayerPrefs.Save();
            jsonString = PlayerPrefs.GetString("highscoreTable");
        }
        //grab the HighScores object
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //the highscores list
        highscores.highScoreEntryList = sortList(highscores);

        //if leaderboard doesn't have 10 places, automatically add it
        if(highscores.highScoreEntryList.Count < 10) {
            //create high score entry
            HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};

            //adding to high scores
            highscores.highScoreEntryList.Add(highscoreEntry);

            //saving the high scores
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
            //if score is greater than 10th place, replace it
        } else if(highscores.highScoreEntryList[highscores.highScoreEntryList.Count-1].score < score) {
            highscores.highScoreEntryList.RemoveAt(highscores.highScoreEntryList.Count-1);
            //create high score entry
            HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};

            //adding to high scores
            highscores.highScoreEntryList.Add(highscoreEntry);

            //saving the high scores
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }
    }

    //button to back up to original scene
    public void backButton() {
        SceneManager.LoadScene("HomeScene");
    }

    //sorts the list in score order.
    private static List<HighscoreEntry> sortList(Highscores highscores) {
        for(int i =0; i < highscores.highScoreEntryList.Count; i++){
            for(int j = i+1; j < highscores.highScoreEntryList.Count; j++) {
                if(highscores.highScoreEntryList[i].score < highscores.highScoreEntryList[j].score) {
                    HighscoreEntry temp = highscores.highScoreEntryList[i];
                    highscores.highScoreEntryList[i] = highscores.highScoreEntryList[j];
                    highscores.highScoreEntryList[j] = temp;
                }
            }
        }
        return highscores.highScoreEntryList;
    }
    //inner class holding the class
    private class Highscores {
        public List<HighscoreEntry> highScoreEntryList;
        
    }

    //inner class holding variables for each high score entry
    [System.Serializable]
    private class HighscoreEntry {
        public float score;
        public string name;
    }
}
