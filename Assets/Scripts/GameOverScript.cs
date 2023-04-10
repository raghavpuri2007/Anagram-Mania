using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    //ui variables from game editor
    public TextMeshProUGUI score;
    public GameObject saveMenu;
    public Button saveButton;
    public InputField input;
    //score of the game
    private float currentScore;
    //sounds
    public AudioSource audioSource;
    public AudioClip gameOverSound;
    //name of user
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        //intialize variables
        currentScore = MainPlayScript.currentScore;
        score.text = "Score: " + currentScore;
        //intialize save menu actions
        saveMenu.SetActive(false);
        saveButton.interactable = true;
        //play game over sounds
        audioSource.PlayOneShot(gameOverSound);
    }

    public void Update() {
        //if escape key is clicked, then quit application
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    //save button clicked
    public void SaveButton() {
        saveMenu.SetActive(true);
    }
    //restart button clicked
    public void Restart() {
        SceneManager.LoadScene("PlayScene");
    }
    //main menu button clicked return to home
    public void MainMenu() {
        SceneManager.LoadScene("HomeScene");
    }
    //if user submits score
    public void submitButton() {
        string name = input.text;
        //check to make sure name is not to long
        if(name.Length > 10) {
            name = name.Substring(0, 10);
        }
        //make sure the name is not empty
        if(name.Length != 0) {
            this.name = name;
            //add person to leaderboard
            HighscoreTable.AddHighScoreEntry(currentScore, name);
            //close save menu actions
            saveMenu.SetActive(false);
            saveButton.interactable = false;
        }
    }
    //return to game over screen
    public void backButton() {
        saveMenu.SetActive(false);
    }
}
