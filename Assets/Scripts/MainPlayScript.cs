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
    //general group
    private List<string> wholeFile;
    public GameObject letterObject;
    public GameObject dashObject;
    private int mode;

    //backgrounds group
    public Sprite[] backgrounds;
    public Image background;

    //displayed objects group
    public static GameObject[] letters;
    public static GameObject[] dashes;

    //timer group
    public TextMeshProUGUI timer;
    private float currentTime;
    private float timerLimit;

    //score group
    public TextMeshProUGUI score;
    public static float currentScore;
    private int streak;

    //words group
    private string currentWord;
    private string scrambledWord;

    //audio group
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
        updateStreak();
        
    }
    //method to create dashes displayed on the screen
    private void createDashes() {
        //instantiate each dash with name and position
        for(int i = 0; i < dashes.Length; i++) {
            GameObject dash = Instantiate(dashObject, new Vector3(0, -100, 0), transform.rotation) as GameObject;
            dash.transform.SetParent(GameObject.FindGameObjectWithTag("background").transform, false);
            dash.transform.position = new Vector3(dash.transform.position.x+((-400+(-200*(mode-3)))+(i*400)), dash.transform.position.y, 0);
            dash.name = $"Dash{i+1}";
            dashes[i] = dash;
        }
    }
    //method to generate a word from text file
    private void generateRandomWord() {
        bool found = false;
        while(!found) {
            //random number in the list
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
    //method to create the letters displayed on the game
    private void createLetters(string word) {
        scrambledWord = word;
        //instantiate each letter with name, position, and display text
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
    //checks if the correct answer is inputted (always is checking in Update())
    bool isCorrectAnswer() {
        bool InCorrectPosition = true;
        //for each letter
        for(int i = 0; i < letters.Length; i++) {
            //for each dash
            for(int j = 0; j < dashes.Length; j++) {
                //position of current dash
                Vector3 position = new Vector3(dashes[j].transform.position.x, dashes[j].transform.position.y+150, 0);
                //check if letter is in a position
                if(letters[i].transform.position == position) {
                    //check if the letter is in the RIGHT position
                    if(scrambledWord[i] != currentWord[j]) {
                        InCorrectPosition = false;
                    }
                    break;
                } else {
                    //if the letter is not sitting on a dash then we automatically know the answer is incorrect
                    if(j == dashes.Length-1) {
                        return false;
                    }
                }
            }
        }
        //if in correct position return true;
        if(InCorrectPosition) {
            streak+=1;
            return true;
        }
        StartCoroutine(ColorChange(Color.red));
        audioSource.PlayOneShot(incorrectAnswerSound);
        for(int i = 0; i < mode; i++) {
            letters[i].transform.position = letters[i].GetComponent<DragAndDrop>().origin;
        }
        streak = 0;
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
    //method if user puts in correct answer
    void correctAnswer() {
        //destroy old letters;
        StartCoroutine(ColorChange(Color.green));
        for(int i = 0; i < letters.Length; i++) {
            Destroy(letters[i]);
        }
        //increase the score by score
        increaseScore(1000);
        //play correct sound
        audioSource.PlayOneShot(correctAnswerSound);
        //generate a new word
        generateRandomWord();
    }

    void updateStreak() {
        ProgressBar.current = streak;
        ProgressBar.streak= streak;
        // if(ProgressBar.streak == ProgressBar.maximum) {
        //     increaseScore(ProgressBar.currentBonus);
        // }
    }
    //method to increase score
    void increaseScore(float amount) {
        currentScore += amount;
        score.text = "Score: " + currentScore.ToString();
    }
    //method to scramble a word
    string shuffle(string word) {
        string newWord = "";
        int index = 0;
        while(!string.IsNullOrEmpty(word)) {
            index = Random.Range(0, word.Length);
            newWord += word[index];
            word = word.Substring(0, index) + word.Substring(index+1);
        }
        //checks to make sure the scrambled word is not the same as the current word
        if(word.Equals(newWord)) {
            return shuffle(word);        
        }
        return newWord;
    }

    //method for timer management
    void timerStuff() {
        //decrease the time
        currentTime -= Time.deltaTime;
        //if time is 0
        if(currentTime <= timerLimit) {
            currentTime = timerLimit;
            timer.color = Color.red;
            SceneManager.LoadScene("GameOverScene");
        }
        //displays current time in the game
        timer.text = "Time: " + currentTime.ToString("0");
    }
}