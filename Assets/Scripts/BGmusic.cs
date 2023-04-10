using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//background music class
public class BGmusic : MonoBehaviour
{
    public static BGmusic instance;
    
    //method that runs when the BG instance starts
    void Awake()
    {
        //destroy gameObject if it is initialied
        if (instance != null)
            Destroy(gameObject);
        
        else
        {
            instance = this;
            instance.GetComponent<AudioSource>().loop = true;
            //makes sure to stay constant through all scenes
            DontDestroyOnLoad(this.gameObject);
        }
        //if the background music is not playing, unpause
        if (!instance.GetComponent<AudioSource>().isPlaying) {
            instance.GetComponent<AudioSource>().UnPause();
        }
    }
}