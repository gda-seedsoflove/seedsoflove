using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControlScript : MonoBehaviour {

    /*  To create a sound, place the following line of code directly under the scripted action that makes sound:
     *  GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>().PlaySound(delay, volume, clip, tag, pause);
     *  You can save GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>() as a script variable
     *      to make things easier on yourself if you need to play many sounds from one script.
     *  You can cut out tag, pause, and delay from the function call and it will still work. Volume and clip must always
     *      be set.
     *      
     *  float Delay: how long (in seconds) the system should wait to play the sound. Defaults to 0.
     *  float Volume: how loud the sound is, from 0 to 1. No default, must be set in method call.
     *  string clip: the exact name of the sound clip being played. No default, must be set in method call.
     *      This script can only play sounds in the Resources/Audio folder.
     *  string tag: sets if the sounds is "Music" or "SFX". It should only be those two values. Defaults to "SFX".
     *  bool pause: sets if the sound should play even when the game is paused. Defaults to false.

     *  If you have any other questions or need more functionality on this script, @me on the discord.
     *  -Thomas.
     */


    public GameObject AudioObject;      //This field should always be set to AudioObject in the inspector.

    /*// Use this for initialization
	void Start () {
		
	}
    *Might not be a needed block, but I'll leave these here just in case.
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public void PlaySound(float delay, float volume, string clipName, string soundTag, bool pauseImmuneToggle)
    {
        ActivateSound(delay, volume, clipName, soundTag, pauseImmuneToggle);
    }

    public void PlaySound(float delay, float volume, string clipName, string soundTag)
    {
        ActivateSound(delay, volume, clipName, soundTag, false);
    }

    public void PlaySound(float delay, float volume, string clipName)
    {
        ActivateSound(delay, volume, clipName, "SFX", false);
    }

    public void PlaySound(float volume, string clipName)
    {
        ActivateSound(0, volume, clipName, "SFX", false);
    }

    public void PlaySound(string clipName)
    {
        ActivateSound(0, 1, clipName, "SFX", false);
    }

    //Don't try to call this method elsewhere.
    void ActivateSound(float delay, float volume, string clipName, string soundTag, bool PauseImmune)
    {
        GameObject instance = Instantiate(AudioObject, gameObject.GetComponent<Transform>());
        instance.GetComponent<SoundPlayer>().ActivateSound
            (delay, volume, Resources.Load<AudioClip>("Audio/" + clipName), soundTag, PauseImmune);
    }

}
//I'm looking into global namespace aliases to make the command you need to use shorter, but I don't really know how.
