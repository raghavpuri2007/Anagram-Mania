using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BGmusic : MonoBehaviour
{
    public static BGmusic instance;
    
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            instance.GetComponent<AudioSource>().loop = true;
            DontDestroyOnLoad(this.gameObject);
        }
        if (!instance.GetComponent<AudioSource>().isPlaying) {
            instance.GetComponent<AudioSource>().UnPause();
        }
    }
}