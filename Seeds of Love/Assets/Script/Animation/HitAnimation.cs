using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimation : MonoBehaviour {

    AnimationClip anim;
	// Use this for initialization
	void Start () {
        anim = new AnimationClip();
        anim.ClearCurves();
        AnimationCurve curve = AnimationCurve.Linear(0.0F, transform.position.x, 2.0F, transform.position.x + 1);
        anim.SetCurve("", typeof(Transform), "localPosition.x", curve);
        anim.name = "Hit";
        GetComponent<Animation>().clip = anim;
        GetComponent<Animator>().Play("Note_Hit_Animation");
        //GetComponent<Animation>().Play("Sheep_Hit");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
