using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 *This Script doesn't work right now as it only exists in branches that don't containt the playerData singleton.
 * If it does, remove the comments and it should work. 
public class DialogueScriptSetter : MonoBehaviour {

    //This script doesn't work right now due to the fact that playerData isn't real yet.
    // Use this for initialization
    void Start() {

		if (playerData.instance.mood < 0.5f)
        {
            gameObject.GetComponent<DialogueParser>().script =
                Resources.Load<TextAsset>("DialogueText/" + SceneManager.GetActiveScene().name + "_Dialogue_Bad.txt");
        }
        else
        {
            gameObject.GetComponent<DialogueParser>().script =
                Resources.Load<TextAsset>("DialogueText/" + SceneManager.GetActiveScene().name + "_Dialogue.txt"); ;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
*/
