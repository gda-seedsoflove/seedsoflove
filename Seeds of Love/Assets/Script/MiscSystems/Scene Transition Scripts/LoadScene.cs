using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Sets variables for fadeScreen and sceneNumber
    SceneFade fadeScreen;
    

    // Finds the SceneFade script that has the desired img
    void Awake() {
        fadeScreen = GameObject.FindObjectOfType<SceneFade>();
    }

    void Start()
    {
        //fadeScreen.Path = GetComponent<ScenePicker>().scenePath;
    }

    public void Trigger() {
        fadeScreen.BeginTransition(GetComponent<SceneFade>().Scenename);
    }
}
