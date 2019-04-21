using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitOnClick : MonoBehaviour {

    public EventSystem EventSystem;
    public GameObject SelectedObject;

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) && EventSystem.currentSelectedGameObject != SelectedObject)
        {
            EventSystem.SetSelectedGameObject(SelectedObject);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape)) && EventSystem.currentSelectedGameObject == SelectedObject)
        {
            ExitGame();
        }
    }
}
