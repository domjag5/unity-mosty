using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSoundtrack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void toggleSoundtrack()
    {
        if (this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().Pause();
        else
            this.GetComponent<AudioSource>().UnPause();
    }
}
