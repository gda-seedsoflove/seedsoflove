using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Personal Notes: review coroutines and ienumerators
 */
public class ScreenFader1 : MonoBehaviour {

    Animator anim;
    bool isFading = false;
    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    public IEnumerator FadeToClear() {
        isFading = true;
        anim.SetTrigger("FadeIn");

        while (isFading) {
            yield return null;
        }
    }

    public IEnumerator FadeToColor() {
        isFading = true;
        anim.SetTrigger("FadeOut");

        while (isFading) {
            yield return null;
        }
    }
    void AnimationComplete() {
        isFading = false;
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("FadeToClear");
            //anim.SetTrigger("FadeIn");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine("FadeToColor");
            //anim.SetTrigger("FadeOut");
        }
	}
}
