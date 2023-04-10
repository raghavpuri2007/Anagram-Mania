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
    //Play Button Clicked
    public void Play() {
        SceneManager.LoadScene("LevelsScene");
    }

    //Leaderboard Button Clicked
    public void Leaderboard() {
        SceneManager.LoadScene("LeaderboardScene");
    }

    //Instructions Button Clicked
    public void Instructions() {
        SceneManager.LoadScene("InstructionsScene");
    }

    //Quit Button Clicked
    public void Quit() {
        Application.Quit();
    }
}
