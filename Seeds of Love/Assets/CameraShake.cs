using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;
    private float shakeAmt = 0;
    
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
            Shake(0.3f, 0.2f);
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
            float shakeX = Random.value * shakeAmt * 2 - shakeAmt;
            float shakeY = Random.value * shakeAmt * 2 - shakeAmt;
            cameraPos.x += shakeX;
            cameraPos.y += shakeY;

            mainCam.transform.position = cameraPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = new Vector3(0, 0, -10);
    }
}
