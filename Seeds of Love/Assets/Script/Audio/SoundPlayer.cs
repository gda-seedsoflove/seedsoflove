using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    //ATTENTION: DO NOT SET THE AUDIOCLIP FIELD OF THIS OBJECT
    //I don't know what it will do, but it might cause everything to explode

    AudioSource SoundSource;                //The object's AudioSource component
    float delay;                            //How much delay before the sound plays
    bool soundStarted;                      //Whether or not the audio has started

	// Use this for initialization
	void Start () {
        //Due to how this and SoundControlScript are structured, ActivateSound will be called before Start.
    }

    //This function is called in SoundControlScript and sets the properties of the AudioObject
    public void ActivateSound(float delayInput, float volumeInput, AudioClip clipInput, string soundTag, bool pauseImmune)
    {

        gameObject.tag = soundTag;
        soundStarted = false;
        SoundSource = gameObject.GetComponent<AudioSource>();
        SoundSource.ignoreListenerPause = pauseImmune;      //Sets whether or not the sound plays when sound is paused.
        SoundSource.clip = clipInput;       //Sets the AudioSource's clip (set to 'none' by default).
        SoundSource.volume = volumeInput;   //Sets the AudioSource's volume (set to 1.0 by default).
        delay = delayInput;                 //Sets the argument value for PlayDelayed
        SoundSource.PlayDelayed(delay);     //Plays the audio after the argument's value
        soundStarted = true;                //The sound is now playing
    }

    // Update is called once per frame
    void Update () {
        if (soundStarted && !SoundSource.isPlaying && !AudioListener.pause)
        {
            Destroy(gameObject);            //Object self destructs
            //But only if the sound started then stopped playing. Does not activate is sound stops due to pauses.
        }
	}

}
