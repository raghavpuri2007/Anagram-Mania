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
    public GameObject powerUpObject;
    private int mode;

    //backgrounds group
    public Sprite[] backgrounds;
    public Image background;

    //displayed objects group
    public static GameObject[] letters;
    public static GameObject[] dashes;
    public static GameObject[] powerUps;

    //timer group
    public TextMeshProUGUI timer;
    private float currentTime;
    private float timerLimit;
    private float currentWordTimer;

    //score group
    public TextMeshProUGUI score;
    public TextMeshProUGUI wordScore;
    public static float currentScore;
    public int streak;

    //words group
    public string currentWord;
    public string scrambledWord;

    //audio group
    public AudioSource audioSource;
    public AudioClip correctAnswerSound;
    public AudioClip incorrectAnswerSound;


    //powerup helpers
    public bool timeFreeze;

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
        currentWordTimer = 0;
        letters = new GameObject[mode];
        dashes = new GameObject[mode];
        powerUps = new GameObject[3];
        timeFreeze = false;
        wordScore.enabled = false;
        background.sprite = backgrounds[mode-3];
        createDashes();
        generateRandomWord();
        createShop();
    }

    // Update is called once per frame
    void Update()
    { 
        timerStuff();
        if(isCorrectAnswer()) {
            correctAnswer();
        }
        updateStreak();
        //escape key pressed
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
        
    }
    //method to create dashes displayed on the screen
    private void createDashes() {
        //instantiate each dash with name and position
        for(int i = 0; i < dashes.Length; i++) {
            GameObject dash = Instantiate(dashObject, new Vector3(0, -250, 0), transform.rotation) as GameObject;
            dash.transform.SetParent(GameObject.FindGameObjectWithTag("background").transform, false);
            dash.transform.position = new Vector3(dash.transform.position.x+((-420+(-210*(mode-3)))+(i*420)), dash.transform.position.y -75, 0);
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
            currentLetter.transform.position = new Vector3(dashes[i].transform.position.x, dashes[i].transform.position.y-200, 0);
            currentLetter.name = $"letter{i+1}";

            TextMeshProUGUI currentLetterText =  currentLetter.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            currentLetterText.text = scrambledWord[i] + "";
            letters[i] = currentLetter;

        }
        
    }
    private void createShop() {
        //create power ups
        float costMultiplier = 100;
        for(int i = 0; i < 3; i++) {
            GameObject currentPowerUp = Instantiate(powerUpObject, transform.position, transform.rotation) as GameObject;
            currentPowerUp.transform.SetParent(GameObject.FindGameObjectWithTag("shop").transform, false);
            // Debug.Log();
            currentPowerUp.transform.position = new Vector3(GameObject.FindGameObjectWithTag("shop").transform.position.x, GameObject.FindGameObjectWithTag("shop").transform.position.y + 150 + (i*-200), 0);
            PowerUpScript script = currentPowerUp.GetComponent<PowerUpScript>();
            if(i == 0) {
                script.name = "Time Freeze";
                script.cost = (1500 - ((mode-3) * (costMultiplier*2)))+ "";
                script.nameColor = new Color32(40, 242, 255, 255);
            } else if(i ==1 ) {
                script.name = "Free Letter";
                script.cost = (500 - ((mode-3)*costMultiplier)) + "";
                script.nameColor = new Color32(169, 250, 108, 255);
            } else if(i == 2) {
                script.name = "Streak Boost";
                script.cost = (1500 - ((mode-3) * (costMultiplier*2)))+ "";
                script.nameColor = new Color32(241, 82, 72, 255);
            }
            //will eventually have array with each power up
            powerUps[i] = currentPowerUp;
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
        //score
        float score = 750 - Mathf.Round(currentWordTimer * 6) + ((mode-3)*250);
        currentWordTimer = 0;
        increaseScore(score);
        StartCoroutine(wordScoreCoroutine(score));
        //play correct sound
        audioSource.PlayOneShot(correctAnswerSound);
        //generate a new word
        generateRandomWord();
    }
    IEnumerator wordScoreCoroutine(float score) {
        wordScore.text = "Score: +" + score;
        wordScore.enabled = true;
        yield return new WaitForSeconds(1f);
        wordScore.enabled = false;
    }
    void updateStreak() {
        ProgressBar.current = streak;
        ProgressBar.streak= streak;
        if(ProgressBar.streak == ProgressBar.maximum) {
            increaseScore(ProgressBar.currentBonus);
        }
    }
    //method to increase score
    public void increaseScore(float amount) {
        currentScore += amount;
        score.text = "Score: " + currentScore.ToString();
    }
    //method to scramble the current word
    string shuffle(string word) {
        //string to hold scrambled word
        string newWord = "";
        int randomIndex = 0;
        while(!string.IsNullOrEmpty(word)) {
            //grab a random index in the word
            randomIndex = Random.Range(0, word.Length);
            //add the random letter to the new word
            newWord += word[randomIndex];
            //remove the random letter from original word
            word = word.Substring(0, randomIndex) + 
            word.Substring(randomIndex+1);
        }
        //checks to make sure shuffle
        //not same as orignal word
        if(currentWord.Equals(newWord)) {
            return shuffle(currentWord);        
        }
        return newWord;
    }


    //method for timer management
    void timerStuff() {
        //decrease the time
        if(!timeFreeze) {
            currentTime -= Time.deltaTime;
            //if time is 0
            if(currentTime <= timerLimit) {
                currentTime = timerLimit;
                timer.color = Color.red;
                SceneManager.LoadScene("GameOverScene");
            }
            //displays current time in the game
        }
        currentWordTimer += Time.deltaTime;
        timer.text = "Time: " + currentTime.ToString("0");
    }
}