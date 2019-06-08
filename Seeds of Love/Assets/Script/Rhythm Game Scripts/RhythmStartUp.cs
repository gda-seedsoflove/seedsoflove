using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmStartUp : MonoBehaviour {

    public GameObject StartUpMenu;
	// Use this for initialization
	void Awake () {
        if (StartUpMenu)
        {
            StartUpMenu.SetActive(true);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && StartUpMenu.active == true)
        {
            StartUpMenu.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
        }
    }

}
