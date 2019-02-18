using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControlScript : MonoBehaviour {

    /*  To create a sound, place the following line of code directly under the scripted action that makes sound:
     *  GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>().PlaySound(delay, volume, clip);
     *  You can save GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>() as a script variable
     *      to make things easier on yourself if you need to play many sounds from one script.
     *  Delay and Volume are both float values, so make sure to add an f to the end of them so C# knows. The clip argument
     *      is the EXACT name of the audio clip in the Audio folder in double quotes. This script can only pull sounds
     *          from the Audio subfolder in the Resources folder.
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

    public void PlaySound(float delay, float volume, string clipName)
    {
        GameObject instance = Instantiate(AudioObject, gameObject.GetComponent<Transform>());
        instance.GetComponent<SoundPlayer>().ActivateSound
            (delay, volume, Resources.Load<AudioClip>("Audio/" + clipName));
        //Line 20: Creates a new instance of the AudioObject as a child of the Sound Controller
        //Line 21: Passes the arguments along to the new object. This keeps the sound manager free to make more
        //  audio as needed. Also, Resources.load takes a string as an argument, so that's easy.
    }
}
