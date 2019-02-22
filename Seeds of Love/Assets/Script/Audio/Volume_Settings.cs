using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Simplified script to handle changinge the volume of
//different sounds. When adding sounds, be sure to change
// the output to either BGM or SFX in AudioMixer
public class Volume_Settings : MonoBehaviour{
	
	public AudioMixer audioMixer;
	
	public void SetBGMVolume(float volume){
		
		audioMixer.SetFloat("BGMVolume", volume);
	}
	
	public void SetSFXVolume(float volume){
		
		audioMixer.SetFloat("SFXVolume", volume);
	}
	
	public void SetMasterVolume(float volume){
		
		audioMixer.SetFloat("MasterVolume", volume);
	}
}