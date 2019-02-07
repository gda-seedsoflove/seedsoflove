using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    SceneFade fadeScreen;
    public int sceneNumber;

    void Awake() {
        fadeScreen = GameObject.FindObjectOfType<SceneFade>();
    }

    public void Trigger() {
        Debug.Log("Begin EndScene");
        fadeScreen.BeginTransition(sceneNumber);
    }
}
