using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmStartUp : MonoBehaviour {

    public GameObject StartUpMenu;
	// Use this for initialization
	void Start () {
        if (StartUpMenu)
        {
            StartUpMenu.SetActive(true);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartUpMenu.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
        }
    }

}
