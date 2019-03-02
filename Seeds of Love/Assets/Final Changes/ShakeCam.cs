using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{

    public Camera mainCam;
    private float shakeAmt = 4;                                     //influences the distance the camera moves from its position
    private float decreaseAmt = 0.5f;
    private float shakeDurat = 1f;
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
            Shaking(3f, 0.5f, 1f);
        }
    }

    void Shaking(float sAmt, float dAmt, float sDurat)
    {
        if (sDurat > 0)
        {
            Vector3 cameraPos = mainCam.transform.position;
            float shakeX = (Random.value * sAmt * 2) - sAmt;
            float shakeY = (Random.value * sAmt * 2) - sAmt;
            cameraPos.x += shakeX;
            cameraPos.y += shakeY;
            mainCam.transform.position = cameraPos;
            sDurat -= Time.deltaTime * dAmt;
        }
        else
        {
            sDurat = 0f;
            mainCam.transform.localPosition = originalPos;
        }
    }
}
