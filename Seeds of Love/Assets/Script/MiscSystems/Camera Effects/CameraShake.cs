using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;
    private float shakeAmt = 4;                                     //influences the distance the camera moves from its position
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
            Shake(0.6f, 8f);
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmt = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    /*
     * Less for y direction
     */
    void BeginShake()
    {
        if (shakeAmt > 0)
        {
            Vector3 cameraPos = mainCam.transform.position;
            float shakeX = (Random.value * (shakeAmt * 2)) - shakeAmt;                //how much the camera shakes in the x direction
            float shakeY = (Random.value * (shakeAmt * 2)) - shakeAmt;                //how much the camera shakes in the y direction
            /*
             * Should I move the resetting code before the addition code?
             */
            cameraPos.x += shakeX;
            cameraPos.y += shakeY;

            if (cameraPos.x > 9.5 || cameraPos.x < -9.5 || cameraPos.y > 3.5 || cameraPos.y < -3.5)
            {
                //Debug.Log("Hello: I am sad");
                mainCam.transform.localPosition = originalPos;
                //Debug.Log("Hello: I am happy");
            }
            else
            {
                //Debug.Log("Goodbye: I am satisfied");
                mainCam.transform.position = cameraPos;                                   //This is needed to move the camera
            }
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = originalPos;                            //resets the camera to the original position
    }
}
