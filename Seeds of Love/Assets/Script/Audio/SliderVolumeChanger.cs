using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//I haven't actually tested these because I don't have headphones on me.

public class SliderVolumeChanger : MonoBehaviour {

    public AudioMixer AudioMixer;
    Slider CurrentSlider;
    string SliderName;
    float slideVal;

	// Use this for initialization
	void Start () {
        //AudioMixer = GetComponent<>().outputAudioMixerGroup.audioMixer;
        CurrentSlider = GetComponent<Slider>();
        SliderName = CurrentSlider.name.Split(' ')[0];
    }

    /*/ Update is called once per frame
	void Update () {
		AudioMixer.SetFloat("SFXVolume", volume);
	}*/

    public void ChangeVolume()
    {
        slideVal = CurrentSlider.value;//hittin some phat null reference

        if(SliderName == "SFX")
        {
            AudioMixer.SetFloat("SFXVolume", slideVal);
        }
        else if(SliderName == "Music")
        {
            AudioMixer.SetFloat("MusicVolume", slideVal);
        }
        else  //assumes it must be a master slider
        {
            AudioMixer.SetFloat("MasterVolume", slideVal);
        }
        Debug.Log(SliderName + " " + slideVal);
    }
}
