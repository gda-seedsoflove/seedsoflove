using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndDespawnPauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public GameObject PausePanel;
    public GameObject optionsPanel;
    public GameObject pButton;

    public void TogglePauseMenu()
    {
        if (optionsPanel.activeSelf == true)
        {
            optionsPanel.SetActive(false);
            PausePanel.SetActive(false);
        }
        else if (PausePanel.activeSelf == false)
        {
            PausePanel.SetActive(true);
        }
        else
        {
            PausePanel.SetActive(false);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            TogglePauseMenu();
        }
    }
}
