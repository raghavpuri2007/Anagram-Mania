using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainScript : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadScene("LevelsScene");
    }

    public void Leaderboard() {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void Options() {
        SceneManager.LoadScene("OptionsScene");
    }

    public void Quit() {
        Application.Quit();
    }
}
