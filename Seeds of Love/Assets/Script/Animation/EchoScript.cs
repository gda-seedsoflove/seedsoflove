using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoScript : MonoBehaviour {

    public float interval;
    private float currinterval;

    public GameObject echo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (currinterval < interval)
        {
            currinterval += Time.deltaTime;
        }
        else
        {
            GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(instance, 1f);
            currinterval = 0;
        }
	}
}
