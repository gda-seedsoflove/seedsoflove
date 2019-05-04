using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    IEnumerator highlightBtn()
    {
        EventSystem myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        myEventSystem.SetSelectedGameObject(null);
        yield return null;
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);

    }


        // Update is called once per frame
        void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            StartCoroutine("highlightBtn");
        }
    }
}
