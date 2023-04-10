using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainScript : MonoBehaviour
{
    public void Update() {
        //escape key pressed
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    public void Play() {
        SceneManager.LoadScene("LevelsScene");
    }

    public void Leaderboard() {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void Options() {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void Quit() {
        Application.Quit();
    }
}
