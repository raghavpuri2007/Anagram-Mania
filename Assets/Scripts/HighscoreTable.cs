using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entry;
    private List<Transform> highScoreEntryTransformList;

    public void Update() {
        //escape key pressed
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    
    private void Awake() {
        entryContainer = transform.Find("HighScoreContainer");
        entry = entryContainer.Find("HighScoreEntry");
        entry.gameObject.SetActive(false);
        
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
        foreach(HighscoreEntry highscore in highscores.highScoreEntryList) {
            CreateHighscoreEntryTransform(highscore, entryContainer, highScoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highScoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 95f;
        Transform entryTransfrom = Instantiate(entry, container);
        entryTransfrom.gameObject.SetActive(true);
        RectTransform entryRectTransform = entryTransfrom.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, entry.GetComponent<RectTransform>().anchoredPosition.y-(templateHeight * transformList.Count));

        int rank = transformList.Count+1;
        Image icon =  entryTransfrom.Find("Icon").GetComponent<UnityEngine.UI.Image>();
        if(rank == 1) {
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(217, 194, 21, 255);
        } else if(rank == 2) {
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(192, 192, 192, 255);
        } else if(rank == 3) {
            entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
            entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
            entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
        }
        string rankString;
        switch(rank) {
            default: 
                rankString = rank + "TH";
                icon.enabled  = false;
                break;
            
            case 1: 
                rankString = "1ST"; 
                icon.color = new Color32(217, 194, 21, 255);
                break;
            case 2: 
                rankString = "2ND"; 
                icon.color = new Color32(192, 192, 192, 255);
                break;
            case 3: 
                rankString = "3RD"; 
                icon.color = new Color32(205, 127, 50, 255);
                break;
        }
        entryTransfrom.Find("Pos").GetComponent<TextMeshProUGUI>().text = rankString;
        float score = highScoreEntry.score;
        entryTransfrom.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
        entryTransfrom.Find("Name").GetComponent<TextMeshProUGUI>().text = highScoreEntry.name;

        transformList.Add(entryTransfrom);
    }
    public static void AddHighScoreEntry(float score, string name) {

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
        for(int i = 0; i < highscores.highScoreEntryList.Count; i++) {
            Debug.Log(highscores.highScoreEntryList[i].score + highscores.highScoreEntryList[i].name);
        }
        if(highscores.highScoreEntryList.Count < 10) {
            //create high score entry
            HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};

            //adding to high scores
            highscores.highScoreEntryList.Add(highscoreEntry);

            //saving the high scores
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
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

    public void backButton() {
        SceneManager.LoadScene("HomeScene");
    }

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

    private class Highscores {
        public List<HighscoreEntry> highScoreEntryList;
        
    }

    [System.Serializable]
    private class HighscoreEntry {
        public float score;
        public string name;
    }
}
