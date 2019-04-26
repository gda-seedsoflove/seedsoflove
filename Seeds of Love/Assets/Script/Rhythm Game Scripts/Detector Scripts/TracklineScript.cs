using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracklineScript : MonoBehaviour {

    public GameObject Followtarget;

	// Use this for initialization
	void Start () {
        transform.position = new Vector2(Followtarget.transform.position.x, transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(Followtarget.transform.position.x,transform.position.y);
	}
}
