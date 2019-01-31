//All of this was stolen from Kenny's ParticleScript

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{

    private AudioSource SFX;
    private bool played;
    public float delay;
    // Use this for initialization
    void Start()
    {
        SFX = gameObject.GetComponent<AudioSource>();
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay <= 0 && played == false)
        {
            SFX.Play();
            played = true;
        }
        else
        {
            delay -= Time.deltaTime;
        }

        if (SFX.isPlaying == false && played == true)
        {
            Destroy(gameObject);
        }
    }
}
