using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainPlayScript : MonoBehaviour
{
    private List<string> wholeFile;
    public GameObject letterObject;
    public GameObject dashObject;
    private int mode;

    public Sprite[] backgrounds;
    public Image background;

    public static GameObject[] letters;
    public static GameObject[] dashes;

    //everything under this is good
    public TextMeshProUGUI timer;
    private float currentTime;
    private float timerLimit;

    public TextMeshProUGUI score;
    public static float currentScore;

    private string currentWord;
    private string scrambledWord;

    public AudioSource audioSource;
    public AudioClip correctAnswerSound;
    public AudioClip incorrectAnswerSound;

    // Start is called before the first frame update
    void Start()
    {
        //initialize File of words
        string readFromFilePath = Application.streamingAssetsPath + "/Words/words.txt";
        wholeFile = File.ReadAllLines(readFromFilePath).ToList();
        mode = PlayerPrefs.GetInt("mode");
        timerLimit = 0;
        currentTime = 45;
        currentScore = 0;
        letters = new GameObject[mode];
        dashes = new GameObject[mode];
        background.sprite = backgrounds[mode-3];
        createDashes();
        generateRandomWord();
    }

    // Update is called once per frame
    void Update()
    {
        timerStuff();
        if(isCorrectAnswer()) {
            correctAnswer();
        }
    }

    private void createDashes() {
        for(int i = 0; i < dashes.Length; i++) {
            GameObject dash = Instantiate(dashObject, new Vector3(0, -100, 0), transform.rotation) as GameObject;
            dash.transform.SetParent(GameObject.FindGameObjectWithTag("background").transform, false);
            dash.transform.position = new Vector3(dash.transform.position.x+((-400+(-200*(mode-3)))+(i*400)), dash.transform.position.y, 0);
            dash.name = $"Dash{i+1}";
            dashes[i] = dash;
        }
    }

    private void generateRandomWord() {
        bool found = false;
        while(!found) {
            int ranNo1= Random.Range(0, wholeFile.Count);
            if(wholeFile[ranNo1].Length == mode) {
                //checks if it is a realistic word (if it contains vowels)

                if(wholeFile[ranNo1].Contains("a") || wholeFile[ranNo1].Contains("e") || wholeFile[ranNo1].Contains("i") || wholeFile[ranNo1].Contains("o") || wholeFile[ranNo1].Contains("u")) {
                    found = true;
                    currentWord = wholeFile[ranNo1];
                    wholeFile.Remove(currentWord);
                }
            }
        }
        createLetters(shuffle(currentWord));
    }
    private void createLetters(string word) {
        scrambledWord = word;
        //instantiate letter1
        for(int i = 0; i < mode; i++) {
            GameObject currentLetter = Instantiate(letterObject, transform.position, transform.rotation) as GameObject;
            currentLetter.transform.SetParent(GameObject.FindGameObjectWithTag("background").transform, false);
            currentLetter.transform.position = new Vector3(dashes[i].transform.position.x, dashes[i].transform.position.y-300, 0);
            currentLetter.name = $"letter{i+1}";

            TextMeshProUGUI currentLetterText =  currentLetter.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            currentLetterText.text = scrambledWord[i] + "";
            letters[i] = currentLetter;

        }
        
    }
    bool isCorrectAnswer() {
        bool InCorrectPosition = true;
        for(int i = 0; i < letters.Length; i++) {
            //i = currentLetter 
            for(int j = 0; j < dashes.Length; j++) {
                Vector3 position = new Vector3(dashes[j].transform.position.x, dashes[j].transform.position.y+150, 0);
                if(letters[i].transform.position == position) {
                    if(scrambledWord[i] != currentWord[j]) {
                        InCorrectPosition = false;
                    }
                    break;
                } else {
                    if(j == dashes.Length-1) {
                        return false;
                    }
                }
            }
        }
        //if in correct position return true;
        if(InCorrectPosition) {
            return true;
        }
        StartCoroutine(ColorChange(Color.red));
        audioSource.PlayOneShot(incorrectAnswerSound);
        for(int i = 0; i < mode; i++) {
            letters[i].transform.position = letters[i].GetComponent<DragAndDrop>().origin;
        }
        return false;
    }
    //change each dash in array
    IEnumerator ColorChange(Color color)
    {
        //Print the time of when the function is first called.
        for(int i = 0; i < dashes.Length; i++) {
            dashes[i].GetComponent<Image>().color = color;
        }
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);

        //After we have waited 5 seconds print the time again.
        for(int i = 0; i < dashes.Length; i++) {
            dashes[i].GetComponent<Image>().color = Color.white;
        }
    }

    void correctAnswer() {
        //destroy old letters;
        StartCoroutine(ColorChange(Color.green));
        for(int i = 0; i < letters.Length; i++) {
            Destroy(letters[i]);
        }
        increaseScore(1000);
        audioSource.PlayOneShot(correctAnswerSound);
        generateRandomWord();
    }
    void increaseScore(float amount) {
        currentScore += amount;
        score.text = "Score: " + currentScore.ToString();
    }
    //method to scramble a word, good
    string shuffle(string word) {
        string newWord = "";
        int index = 0;
        while(!string.IsNullOrEmpty(word)) {
            index = Random.Range(0, word.Length);
            newWord += word[index];
            word = word.Substring(0, index) + word.Substring(index+1);
        }
        return newWord;
    }
    //GOOD
    void timerStuff() {
        currentTime -= Time.deltaTime;
        if(currentTime <= timerLimit) {
            currentTime = timerLimit;
            timer.color = Color.red;
            SceneManager.LoadScene("GameOverScene");
        }
        timer.text = "Time: " + currentTime.ToString("0");
    }
}