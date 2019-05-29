using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveSecondWait : MonoBehaviour {

    SceneFade fadeScreen;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        fadeScreen = GameObject.FindObjectOfType<SceneFade>();
        Debug.Log("Begin EndScene");
        fadeScreen.BeginTransition(fadeScreen.Scenename);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
