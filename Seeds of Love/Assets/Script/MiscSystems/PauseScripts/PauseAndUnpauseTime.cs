using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseAndUnpauseTime : MonoBehaviour {


	// Use this for initialization
	void Start () {
        
    }

    public void TogglePause(float t)
    {
        float currTime = Time.timeScale; 
        if(currTime != 0.0)
        {
            Time.timeScale = 0.0f;

        } else
        {
            Time.timeScale = t;
        }
    }
	
	// Update is called once per frame
	void Update () {
       
    }
}
