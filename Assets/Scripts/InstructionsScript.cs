using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour
{
    public void Update() {
        //if escape key pressed, close game
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    //return to home screen
    public void backButton() {
        SceneManager.LoadScene("HomeScene");
    }
}
