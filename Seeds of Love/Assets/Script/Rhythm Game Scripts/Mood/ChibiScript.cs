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
        //Debug.Log(BeatmapReader.GetComponent<BeatmapReader>().bpm/60);
        GetComponent<Animator>().speed = BeatmapReader.GetComponent<BeatmapReader>().bpm/60;
        if (PlayerData.instance != null)
        {
            PrevMood = PlayerData.instance.Mood;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerData.instance != null && BeatmapReader.GetComponent<BeatmapReader>().songEnd == false)
        {
            if (PlayerData.instance.Mood < .5f || SadTime >= 0)
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

            if (PlayerData.instance.Mood < PrevMood)
            {
                SadTime += .35f; // Sad time will increase but .1 seconds
            }
            else if(SadTime >= 0)
            {
                SadTime -= Time.deltaTime;
            }
            PrevMood = PlayerData.instance.Mood;
        }
        else
        {
            Debug.LogWarning("ChibiScript: PlayerData(required) does not exist in this context. ");
        }
	}
}
