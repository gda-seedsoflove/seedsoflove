using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerData.instance != null)
        {
            if (PlayerData.instance.Mood < .2f)
            {
                //sad face
            }
            else if (PlayerData.instance.Mood > .8f)
            {
                //Really happy
            }
            else
            {
                //Normal?
            }
        }
        else
        {
            Debug.LogWarning("ChibiScript: PlayerData(required) does not exist in this context. ");
        }
	}
}
