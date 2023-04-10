using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SwapScenes : MonoBehaviour
{
    void Update()
    {
 
        if (SceneManager.GetActiveScene().name == "GameOverScene") {
            BGmusic.instance.GetComponent<AudioSource>().Pause();
        } else {
            BGmusic.instance.GetComponent<AudioSource>().UnPause();
        }
    }
}