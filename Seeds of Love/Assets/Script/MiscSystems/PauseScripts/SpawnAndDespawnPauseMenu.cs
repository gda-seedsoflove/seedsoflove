using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnAndDespawnPauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public bool paused = false;

    public GameObject PausePanel;
    public GameObject optionsPanel;
    public GameObject pButton;

    public void TogglePauseMenu()
    {

        if (PausePanel.activeSelf || optionsPanel.activeSelf)
        {
            Time.timeScale = 1.0f;
            AudioListener.pause = true;
            paused = false;

            optionsPanel.SetActive(false);
            PausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            AudioListener.pause = false;
            paused = true;

            optionsPanel.SetActive(false);
            PausePanel.SetActive(true);
            GameObject.Find("Pause_Menu_Panel/Return_Button").GetComponent<Button>().Select();
        }
        Debug.Log(AudioListener.pause);

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("escape"))
        {
            TogglePauseMenu();
        }
        Debug.Log(AudioListener.pause);
    }


}
