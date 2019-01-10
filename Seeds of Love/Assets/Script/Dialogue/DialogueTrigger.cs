using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to start the dialogue sequence
public class DialogueTrigger : MonoBehaviour {

    
    //finds the Dialogue Manager object and starts the Dialogue
    public void TriggerDialogue()
    {
        DialogueManager x = (DialogueManager) FindObjectOfType(typeof(DialogueManager));
        x.StartDialogue();
    }
}
