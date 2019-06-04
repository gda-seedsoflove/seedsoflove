using System;
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

    public GameObject Credits;
    public GameObject GameTitle;
    public GameObject ThankYou;

    // Use this for initialization
    void Awake () {

        // Setting script states
        Credits.GetComponent<CreditsScroll>().enabled = false;
        GameTitle.GetComponent<FadeInScript>().enabled = false;
        //ThankYou.GetComponent<FadeOutScript>().enabled = false;
       
		
	}

    void Start()
    {
        Debug.Log("Before DoTheThings");
        StartCoroutine("DoTheThing");
        Debug.Log("Function Called");
    }

    IEnumerator DoTheThing()
    {
        Debug.Log("Before 1st wait");
        yield return new WaitForSeconds(CreditsFadeInTime);
        Debug.Log("After Wait");
        GameTitle.GetComponent<FadeInScript>().enabled = true;
        yield return new WaitForSeconds(ScrollStartTime);
        Debug.Log("Start Scrolling");
        Credits.GetComponent<CreditsScroll>().enabled = true;
        
        while (ThankYou.transform.position.y < 0)
        {
            yield return null;
        }
            
        Credits.GetComponent<CreditsScroll>().enabled = false;

        yield return new WaitForSeconds(CreditsFadeOutTime);
        ToTitle();

        Debug.Log("End of function");

    }

    void ToTitle()
    {
        //Write this function to go back to title
    }

    // Update is called once per frame
    void Update () {
		
	}
}
