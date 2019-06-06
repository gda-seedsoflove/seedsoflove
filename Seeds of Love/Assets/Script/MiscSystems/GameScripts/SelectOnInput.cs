using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem EventSystem;
    public GameObject SelectedObject;

    private bool buttonselected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && buttonselected == false && EventSystem.currentSelectedGameObject == false)
        {
            EventSystem.SetSelectedGameObject(SelectedObject);
            buttonselected = true;
        }
        if (EventSystem.currentSelectedGameObject == false && buttonselected == true)
        {
            buttonselected = false;
        }
        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
        {

        }
	}


}
