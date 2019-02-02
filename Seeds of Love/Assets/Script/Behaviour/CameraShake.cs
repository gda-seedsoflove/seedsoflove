using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;
    private float shakeAmt = 1;                                     //influences the distance the camera moves from its position
    Vector3 originalPos = new Vector3(0, 0, -10);                   //the original position of the main camera
    
    void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(1f, 0.9f);
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmt = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmt > 0)
        {
            Vector3 cameraPos = mainCam.transform.position;
            float shakeX = (Random.value * (shakeAmt * 2)) - shakeAmt;                //how much the camera shakes in the x direction
            float shakeY = (Random.value * (shakeAmt * 2)) - shakeAmt;                //how much the camera shakes in the y direction
            /*
             * Do some if statements for the camera positions, but I am too lazy and tired to do math
             */
            cameraPos.x += shakeX;
            cameraPos.y += shakeY;

            mainCam.transform.position = cameraPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = originalPos;                            //resets the camera to the original position
    }
}
