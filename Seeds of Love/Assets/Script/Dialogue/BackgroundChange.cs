using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour {

    /*
     * Background one will start with alpha 1 while two will start with 0
     * When the swapbackground function is called, the alpha values will switch
     */

    public GameObject Background1;
    public GameObject Background2;

    public bool isBackground2;

	// Use this for initialization
	void Start () {
        Color bgcolor1 = Background1.GetComponent<SpriteRenderer>().color;
        Background1.GetComponent<SpriteRenderer>().color = new Color(bgcolor1.r, bgcolor1.g, bgcolor1.b, 1);

        Color bgcolor2 = Background2.GetComponent<SpriteRenderer>().color;
        Background2.GetComponent<SpriteRenderer>().color = new Color(bgcolor2.r, bgcolor2.g, bgcolor2.b, 0);
    }
	
	public void SwapBackground()
    {
        if (isBackground2)
        {
            Color bgcolor1 = Background1.GetComponent<SpriteRenderer>().color;
            Background1.GetComponent<SpriteRenderer>().color = new Color(bgcolor1.r, bgcolor1.g, bgcolor1.b, 1);

            Color bgcolor2 = Background2.GetComponent<SpriteRenderer>().color;
            Background2.GetComponent<SpriteRenderer>().color = new Color(bgcolor2.r, bgcolor2.g, bgcolor2.b, 0);
            isBackground2 = !isBackground2;
        }
        else
        {
            Color bgcolor1 = Background1.GetComponent<SpriteRenderer>().color;
            Background1.GetComponent<SpriteRenderer>().color = new Color(bgcolor1.r, bgcolor1.g, bgcolor1.b, 0);

            Color bgcolor2 = Background2.GetComponent<SpriteRenderer>().color;
            Background2.GetComponent<SpriteRenderer>().color = new Color(bgcolor2.r, bgcolor2.g, bgcolor2.b, 1);
            isBackground2 = !isBackground2;
        }
    }
}
