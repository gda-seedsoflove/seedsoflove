using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour {

    public float ScrollSpeed;
    float OriginalScrollSpeed;

    // Use this for initialization
    void Start () {

        OriginalScrollSpeed = ScrollSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        Vector2 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector2(pos.x, pos.y+ScrollSpeed);

        if (Input.GetKey(KeyCode.Space))
        {
            ScrollSpeed = 0.3f;
        }
        else
        {
            ScrollSpeed = OriginalScrollSpeed;
        }
    }
}
