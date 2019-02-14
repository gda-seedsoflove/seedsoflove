using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndUnpauseTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void TogglePause()
    {
        if(Time.timeScale == 1.0)
        {
            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        } else
        {
            Time.timeScale = Mathf.Approximately(Time.timeScale, 1.0f) ? 0.0f : 1.0f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            TogglePause();
        }
    }
}
