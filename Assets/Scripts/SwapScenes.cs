using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Class to keep music running correctly
public class SwapScenes : MonoBehaviour
{
    void Update()
    {
        //only if the scene is game over, stop the music
        if (SceneManager.GetActiveScene().name == "GameOverScene") {
            BGmusic.instance.GetComponent<AudioSource>().Pause();
        //every other scene should have music playing
        } else {
            BGmusic.instance.GetComponent<AudioSource>().UnPause();
        }
    }
}