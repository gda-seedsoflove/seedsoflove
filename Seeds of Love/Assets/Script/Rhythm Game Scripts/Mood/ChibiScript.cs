using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tests.Interactive.NoteTimingTest;

[RequireComponent(typeof(Animator))]
public class ChibiScript : MonoBehaviour {

    public GameObject BeatmapReader;
    public Sprite HappyChibi;
    public Sprite NormalChibi;
    public Sprite SadChibi;

    private float PrevMood;
    private float SadTime;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().speed = BeatmapReader.GetComponent<BeatmapReader>().bpm/60;
        if (PlayerData.instance != null)
        {
            PrevMood = PlayerData.instance.Mood;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Checks if PlayData exists and if the song is playing. Chibis cannot change their faces unless the song is playing.
        //prevents faces from changing before the song starts
        if (PlayerData.instance != null && BeatmapReader.GetComponent<BeatmapReader>().songEnd == false)
        {
            if (PlayerData.instance.Mood < .5f || SadTime > 0) // if Mood is below the winning threshold (<50%), then the sad chibi replaces the current one
            {
                GetComponent<SpriteRenderer>().sprite = SadChibi;
            }
            else if (PlayerData.instance.Mood > .8f)
            {
                GetComponent<SpriteRenderer>().sprite = HappyChibi;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = NormalChibi;
            }

            if (PlayerData.instance.Mood < PrevMood) // If there is a drop in the mood, the chibi flashes sad face for a beat interval.
            {
                //Debug.Log(1/(float)(BeatmapReader.GetComponent<BeatmapReader>().bpm / 60);
                SadTime += 1/(float)(BeatmapReader.GetComponent<BeatmapReader>().bpm / 60) + BeatmapReader.GetComponent<NoteGenerator>().HitTimeThreshold * 10/3; // Sad time will increase but .1 seconds
            }
            else if(SadTime >= 0)
            {
                SadTime -= Time.deltaTime;
            }

            if (PlayerData.instance.Mood > PrevMood)
            {
                SadTime = 0;
            }

            PrevMood = PlayerData.instance.Mood;
        }
        else
        {
            //Debug.LogWarning("ChibiScript: PlayerData(required) does not exist in this context. ");
        }
	}
}
