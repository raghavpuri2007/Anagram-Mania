using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenuScript : MonoBehaviour
{
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

    public void PauseGame() {
        pauseMenu.SetActive(true);
        //change to getting the object, rather than pulling from MainPlayScript
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            MainPlayScript.letters[i].SetActive(false);
            MainPlayScript.dashes[i].SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            MainPlayScript.letters[i].SetActive(true);
            MainPlayScript.dashes[i].SetActive(true);
        }
        Time.timeScale= 1f;
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOverScene");
    }
}
