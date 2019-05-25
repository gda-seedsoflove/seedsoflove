using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MoodMeterScript : MonoBehaviour {

    public float value = 0;
    public float targetvalue = 0;
    public float percentage;
    private float maxvalue;

    public Slider moodbar;
    public List<GameObject> moodwords;
    public GameObject ParticleEffect;
    private GameObject particleeffect;

    public float delay;

    private Color fillcolor;
    private Image fill;

    private float deviation;        //Distance in mood how far apart the moodwords are

	// Use this for initialization
	void Start () {
        maxvalue = 100;
        fill = moodbar.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();
        fillcolor = fill.color;
        if (moodwords.Count >= 2)
        {
            deviation = maxvalue / (moodwords.Count - 1);
        }
        else
        {
            deviation = 0;
        }

        if (ParticleEffect != null)
        {
            particleeffect = (GameObject)Instantiate(ParticleEffect, transform.position, Quaternion.identity);
            particleeffect.GetComponent<ParticleSystem>().Stop();
        }
    }
	
	// Update is called once per frame
	void Update () {
        targetvalue = Mathf.Clamp(targetvalue, 0, maxvalue);
        value = Mathf.Lerp(value,targetvalue,Time.deltaTime*3/2);
        value = Mathf.Clamp(value,0,maxvalue);
        percentage = value / maxvalue;
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

        if (targetvalue == maxvalue) // Moodbar color
        {            
            fill.color = new Color(fillcolor.r*1.3f, fillcolor.g*1.3f, fillcolor.b*1.3f);
            if (particleeffect && particleeffect.GetComponent<ParticleSystem>().isPlaying == false)
            {
                particleeffect.gameObject.SetActive(true);
                particleeffect.GetComponent<ParticleSystem>().Play();
            }

        }
        else
        {
            float value = Mathf.Lerp(.80f,1.1f, Mathf.Clamp(this.value/this.maxvalue, 0,1));
            fill.color = new Color(fillcolor.r * value, fillcolor.g * value, fillcolor.b * value);
            if (particleeffect && particleeffect.GetComponent<ParticleSystem>().isPlaying == true)
            {
                particleeffect.gameObject.SetActive(false);
                particleeffect.GetComponent<ParticleSystem>().Stop();
            }
        }

        for (int i = 0; i < moodwords.Count;i++)
        {
            try
            {
                float wordmoodvalue = i * deviation;
                Text text = moodwords[i].GetComponent<Text>();
                float alpha = 1 - (Mathf.Abs(wordmoodvalue - value) / deviation);
                //Debug.Log(Mathf.Clamp(alpha, 0, 1));
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp(alpha, .1f, 1));
            }
            catch { }
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
        targetvalue = Mathf.Clamp(targetvalue, 0, maxvalue);
        return targetvalue;
    }

    /**
     * Returns the mood percentage ranging from 0 to 1
     */
    public float GetMoodPercentage()
    {
        targetvalue = Mathf.Clamp(targetvalue, 0, maxvalue);
        return targetvalue/maxvalue;
    }
}
