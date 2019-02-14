using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePauseMenu : MonoBehaviour {

    public GameObject PausePanel;
    public GameObject pButton;

    // Use this for initialization
    void Start () {
        GetComponent<Button>().onClick.AddListener(HidePauseMenu);
    }

    public void HidePauseMenu()
    {
            if (PausePanel.activeSelf == false)
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
		
	}
}
