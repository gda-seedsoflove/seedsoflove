using UnityEngine;

public class VolumeValueChange : MonoBehaviour {

    // Reference to Audio Source component
    private AudioSource audioSrc;

    // Music volume variable that will be modified
    // by dragging slider knob
    private float musicVolume = 1f;
    private bool soundOn;
    public double delay;

	// Use this for initialization
	void Start () {

        // Assign Audio Source component to control it
        audioSrc = GetComponent<AudioSource>();
        SetVolume(audioSrc.volume);
        soundOn = false;

    }
	
	// Update is called once per frame
	void Update () {

        if(delay <= 0 && !soundOn)//GetComponent<AudioSource>().isPlaying == false)
        {
            GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>().PlaySound(0, musicVolume, "Contrafact_Ver3", "Music");
            //GetComponent<AudioSource>().Play();
            soundOn = true;

        }
        else
        {
            delay -= Time.deltaTime;
        }
        // Setting volume option of Audio Source to be equal to musicVolume
        audioSrc.volume = musicVolume;
	}

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
