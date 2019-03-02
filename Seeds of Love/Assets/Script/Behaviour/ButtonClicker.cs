using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicker : MonoBehaviour {

    public CameraShake script;                      //Select the main camera for the script

    private void OnMouseDown()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.r = 0;
        GetComponent<SpriteRenderer>().color = tmp;
        //GetComponent<CameraShake>().Shake(0.5f, 0.5f);
        script.Shake(3f, 2f);
    }
}
