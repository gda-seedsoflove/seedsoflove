using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnPause : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(UnPauseTime);
    }

    public void UnPauseTime() 
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 1.0f) ? 0.0f : 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
