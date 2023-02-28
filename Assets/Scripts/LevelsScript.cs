using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsScript : MonoBehaviour
{
    public void EasyMode() {
        PlayerPrefs.SetInt("mode", 3);
        SceneManager.LoadScene("PlayScene");
    }

    public void MediumMode() {
        PlayerPrefs.SetInt("mode", 4);
        SceneManager.LoadScene("PlayScene");
    }

    public void HardMode() {
        PlayerPrefs.SetInt("mode", 5);
        SceneManager.LoadScene("PlayScene");
    }

    public void ExtremeMode() {
        PlayerPrefs.SetInt("mode", 6);
        SceneManager.LoadScene("PlayScene");
    }

    public void backButton() {
         SceneManager.LoadScene("HomeScene");
    }
}
