using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tests.Interactive.NoteTimingTest;

[RequireComponent(typeof(Animator))]
public class ChibiScript : MonoBehaviour {

    public GameObject BeatmapReader;
	// Use this for initialization
	void Start () {
        //Debug.Log(BeatmapReader.GetComponent<BeatmapReader>().bpm/60);
        GetComponent<Animator>().speed = BeatmapReader.GetComponent<BeatmapReader>().bpm/60;
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerData.instance != null)
        {
            if (PlayerData.instance.Mood < .2f)
            {
                //sad face
            }
            else if (PlayerData.instance.Mood > .8f)
            {
                //Really happy
            }
            else
            {
                //Normal?
            }
        }
        else
        {
            Debug.LogWarning("ChibiScript: PlayerData(required) does not exist in this context. ");
        }
	}
}
