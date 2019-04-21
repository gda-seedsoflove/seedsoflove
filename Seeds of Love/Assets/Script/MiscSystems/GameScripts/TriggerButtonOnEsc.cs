using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class TriggerButtonOnEsc : MonoBehaviour {

    public EventSystem EventSystem;
    public GameObject SelectedObject;
    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            EventSystem.SetSelectedGameObject(SelectedObject);
            EventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
