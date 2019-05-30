using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutScript : MonoBehaviour
{

    SpriteRenderer Image;
    public float FadeOutSpeed;

    // Use this for initialization
    void Start()
    {

        Image = GetComponent<SpriteRenderer>();
        Color c = Image.material.color;
        c.a = 0f;
        Image.material.color = c;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        for (float f = 1; f >= -0.05f; f -= FadeOutSpeed)
        {
            Color c = Image.material.color;
            c.a = f;
            Image.material.color = c;
            yield return new WaitForSeconds(0.05f);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
