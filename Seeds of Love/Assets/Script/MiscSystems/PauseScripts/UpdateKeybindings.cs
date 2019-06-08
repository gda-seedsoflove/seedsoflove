using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeybindings : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ArrowKeys()
    {
        PlayerData.instance.ArrowPreset();
    }

    public void WASD()
    {
        PlayerData.instance.WASDPreset();
    }

    public void JKL()
    {
        PlayerData.instance.JKLPreset();
    }
}
