using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MoodMeterScript : MonoBehaviour {

    public float value = 0;
    public float targetvalue = 0;
    public float percentage;

    public Slider moodbar;
    public float delay;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        targetvalue = Mathf.Clamp(targetvalue, 0, 100);
        value = Mathf.Lerp(value,targetvalue,Time.deltaTime);
        value = Mathf.Clamp(value,0,100);
        percentage = value / 100f;
        moodbar.value = percentage;
        if (delay <= 0)
        {
            //Debug.Log("play");
            var anims = new List<GameObject>();
            anims.AddRange(GameObject.FindGameObjectsWithTag("Chibi"));
            for (int i = 0; i < anims.Count;i++)
            {
                anims[i].GetComponent<Animator>().Play("Jumping");
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
	}

    /**
     * Adds a value to the mood. Max is 100, min is 0
     */
    public void AddMood(float amount)
    {
        targetvalue += amount;
    }

    /**
     * Returns the mood value ranging from 0 to 100
     */
    public float GetMood()
    {
        return targetvalue;
    }

    /**
     * Returns the mood percentage ranging from 0 to 1
     */
    public float GetMoodPercentage()
    {
        return percentage;
    }
}
