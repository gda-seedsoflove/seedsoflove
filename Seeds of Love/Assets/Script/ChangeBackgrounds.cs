using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgrounds : MonoBehaviour {

    public GameObject oldBRound;
    public GameObject newBRound;
	// Use this for initialization
	void Start () {
        oldBRound.SetActive(false);
        newBRound.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
