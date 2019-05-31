using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    // Sets variables for fadeScreen and sceneNumber
    SceneFade fadeScreen;
    public int sceneNumber;

    // Finds the SceneFade script that has the desired img
    void Awake() {
        fadeScreen = GameObject.FindObjectOfType<SceneFade>();
    }

    public void Trigger() {
        Debug.Log("Begin EndScene");
        fadeScreen.BeginTransition(sceneNumber);
    }
}
