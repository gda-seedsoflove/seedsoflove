using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScript : MonoBehaviour {

    Image Image;
    public float FadeInSpeed;
    private Vector2 scale;

    void Awake()
    {

        Image = GetComponent<Image>();
        Color c = Image.material.color;
        c.a = 0f;
        Image.material.color = c;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.01f; f <= 1; f += FadeInSpeed) {
            Color c = Image.material.color;
            c.a = f;
            Image.material.color = c;
            yield return new WaitForSeconds(0.01f);

        }
    }

    public Image GetImage()
    {
        return Image;
    }
}
