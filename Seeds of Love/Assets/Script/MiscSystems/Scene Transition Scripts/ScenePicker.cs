using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScenePicker : MonoBehaviour
{
    [SerializeField]
    public string scenePath;

    public void ChangePath(ScenePicker sp)
    {
        scenePath = sp.scenePath;
    }
}
