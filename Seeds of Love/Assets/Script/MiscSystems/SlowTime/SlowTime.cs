using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour {

    public float newTime;

    // Use this for initialization
    void Start() {

    }

    public void SlowDownTime()
    {

            Time.timeScale = newTime;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
