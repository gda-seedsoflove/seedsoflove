using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchToTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(SwapToTitleScreen);
    }

    public void SwapToTitleScreen()  
    {
            SceneManager.LoadScene(sceneName: "Title");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
