using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

    bool touching = false;
    float time;
    GameObject thing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (touching)
        {
            time += Time.deltaTime;

        }
        if(time >= .5)
        {
            Destroy(thing);
            touching = false;
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        touching = true;
        thing = collision.gameObject;
        
    }
}
