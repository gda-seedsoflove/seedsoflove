using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Created by Justin Morales
*
* This is created to manage the order in which the scripts for the credits go off.
*
* DEPENDANCIES:
* This script will depend on there being a Credits GO(Game Object) with a Credits Scroll script attached to it.
* In Credits it should have a Game Title GO with an image and a Thank You with a text.
* These have FadeIn and FadeOut scripts respectively.
*/

public class CreditsEvent : MonoBehaviour {

    public int CreditsFadeInTime; // Wait for fade in
    public int ScrollStartTime; // Wait to start scrolling up
    public int CreditsFadeOutTime; // Wait for fade out

    // Use this for initialization
    void Start () {

        //Setting Dependancies
        GameObject Credits = GameObject.Find("Credits");
        GameObject GameTitle = GameObject.Find("Credits/Game Title");
        GameObject ThankYou = GameObject.Find("Credits/GameObject/Thank You");

        // Setting script states
        Credits.GetComponent<CreditsScroll>().enabled = false;
        GameTitle.GetComponent<FadeInScript>().enabled = false;
        ThankYou.GetComponent<FadeOutScript>().enabled = false;
        Debug.Log("Before DoTheThings");
        StartCoroutine("DoTheThing");
        Debug.Log("Function Called");
		
	}

    IEnumerator DoTheThing()
    {
        GameObject Credits = GameObject.Find("Credits");
        GameObject GameTitle = GameObject.Find("Credits/Game Title");
        GameObject ThankYou = GameObject.Find("Credits/GameObject/Thank You");
        Debug.Log("Before 1st wait");
        yield return new WaitForSeconds(CreditsFadeInTime);
        Debug.Log("After Wait");
        GameTitle.GetComponent<FadeInScript>().enabled = true;
        yield return new WaitForSeconds(ScrollStartTime);
        Credits.GetComponent<CreditsScroll>().enabled = true;
        /* Wait for distance threshold on GO Credits or ThankYou
         *
         *
         *
         */
        Credits.GetComponent<CreditsScroll>().enabled = false;
        yield return new WaitForSeconds(CreditsFadeOutTime);
        ThankYou.GetComponent<FadeOutScript>().enabled = true;
        Debug.Log("End of function");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
