using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour
{
    public void Update() {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    public void backButton() {
        SceneManager.LoadScene("HomeScene");
    }
}
