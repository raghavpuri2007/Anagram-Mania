using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    public TextMeshProUGUI score;
    public GameObject saveMenu;
    public Button saveButton;
    public InputField input;
    private float currentScore;
    public AudioSource audioSource;
    public AudioClip gameOverSound;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = MainPlayScript.currentScore;
        score.text = "Score: " + currentScore;
        saveMenu.SetActive(false);
        saveButton.interactable = true;
        audioSource.PlayOneShot(gameOverSound);
    }

    public void Update() {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    public void SaveButton() {
        saveMenu.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("PlayScene");
    }

    public void MainMenu() {
        SceneManager.LoadScene("HomeScene");
    }
    public void submitButton() {
        string name = input.text;
        if(name.Length > 10) {
            name = name.Substring(0, 10);
        }
        if(name.Length != 0) {
            this.name = name;
            HighscoreTable.AddHighScoreEntry(currentScore, name);
            saveMenu.SetActive(false);
            saveButton.interactable = false;
        }
    }

    public void backButton() {
        saveMenu.SetActive(false);
    }
}
