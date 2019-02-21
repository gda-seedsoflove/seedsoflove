using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour {

    //Remember! Volumes can only go from 0.0 to 1.0!
    //Also, I don't know if this works. It should, but I don't know if it does.

    float SFXlast;
    float Musiclast;   

	// Use this for initialization
	void Start () {
        SFXlast = 1;
        Musiclast = 1;
	}
	
	/*// Update is called once per frame
	void Update () {
		
	}*/

    public void GeneralVolumeChange(float genVolume)
    {
        AudioListener.volume = genVolume;
    }

    public void SFXVolumeChange(float newVolume)
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("SFX");
        int soundLength = 0;
        while (soundLength < sounds.Length)
        {
            sounds[soundLength].GetComponent<AudioSource>().volume =
                newVolume * (sounds[soundLength].GetComponent<AudioSource>().volume / SFXlast);
            soundLength++;
        }

        if (newVolume == 0) { Musiclast = 1; }
        else { SFXlast = newVolume; }
    }

    public void MusicVolumeChange(float newVolume)
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("Music");
        int soundLength = 0;
        while (soundLength < sounds.Length)
        {
            sounds[soundLength].GetComponent<AudioSource>().volume =
                newVolume * (sounds[soundLength].GetComponent<AudioSource>().volume / Musiclast);
            soundLength++;
        }

        if (newVolume == 0) { Musiclast = 1; }
        else{ Musiclast = newVolume; }
    }
}
