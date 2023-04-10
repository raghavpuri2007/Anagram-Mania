using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Levels Select Screen
public class LevelsScript : MonoBehaviour
{

    public void Update() {
        //if escape key clicked, close game
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    //easy mode clicked, load easy mode (3 letter word)
    public void EasyMode() {
        PlayerPrefs.SetInt("mode", 3);
        SceneManager.LoadScene("PlayScene");
    }

    //medium mode clicked, load medium mode (4 letter word)
    public void MediumMode() {
        PlayerPrefs.SetInt("mode", 4);
        SceneManager.LoadScene("PlayScene");
    }

    //hard mode clicked, load hard mode (5 letter word)
    public void HardMode() {
        PlayerPrefs.SetInt("mode", 5);
        SceneManager.LoadScene("PlayScene");
    }
    //extreme mode clicked, load extreme mode (6 letter word)
    public void ExtremeMode() {
        PlayerPrefs.SetInt("mode", 6);
        SceneManager.LoadScene("PlayScene");
    }

    //return to home screen
    public void backButton() {
         SceneManager.LoadScene("HomeScene");
    }
}
