using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {

    private ParticleSystem ps;
    private bool played;
    public float delay;
	// Use this for initialization
	void Start () {
        ps = gameObject.GetComponent<ParticleSystem>();
        played = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (delay <= 0 && played == false)
        {
            ps.Play();
            played = true;
        }
        else
        {
            delay -= Time.deltaTime;
        }

        if(ps.isPlaying == false && played == true)
        {
            Destroy(gameObject);
        }
    }
}
