using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Pause Menu in the MainPlayScene
public class PauseMenuScript : MonoBehaviour
{
    //pause menu game object
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //method to pause the game if it is clicked
    public void PauseGame() {
        pauseMenu.SetActive(true);
        //change to getting the object, rather than pulling from MainPlayScript
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            MainPlayScript.letters[i].SetActive(false);
            MainPlayScript.dashes[i].SetActive(false);
            if(i < MainPlayScript.powerUps.Length) {
                MainPlayScript.powerUps[i].SetActive(false);
            }
        }
        //stop running
        Time.timeScale = 0f;
    }
    //method to resume the game if it is clicked
    public void ResumeGame() {
        pauseMenu.SetActive(false);
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            //reactivate all the variables and objects
            MainPlayScript.letters[i].SetActive(true);
            MainPlayScript.dashes[i].SetActive(true);
            if(i < MainPlayScript.powerUps.Length) {
                MainPlayScript.powerUps[i].SetActive(true);
            }
        }
        //resume frame running
        Time.timeScale= 1f;
    }

    //method to quit current game and go to game over screen
    public void QuitGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOverScene");
    }
}
