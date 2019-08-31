using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePauseMenu : MonoBehaviour {

    public GameObject PausePanel;
    public GameObject pButton;
    public SpawnAndDespawnPauseMenu TogglePause;

    // Use this for initialization
    void Start () {
        TogglePause = GameObject.Find("UI").GetComponent<SpawnAndDespawnPauseMenu>();
        GetComponent<Button>().onClick.AddListener(HidePauseMenu);
        
    }

    public void HidePauseMenu()
    {
        
        TogglePause.TogglePauseMenu();
        Debug.Log(AudioListener.pause);
        AudioListener.pause = false;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
