using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCG : MonoBehaviour {

    [SerializeField]
    DialogueManager dm;

    Animator anim;

    bool killToggle;

	// Use this for initialization
	void Start () {
        killToggle = true;
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(dm.dialogue == "I... I... " && killToggle)
        {
            killToggle = false;
            StartCoroutine(FadeOut());
        }
	}

    IEnumerator FadeOut()
    {
        anim.SetBool("Kill", true);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
