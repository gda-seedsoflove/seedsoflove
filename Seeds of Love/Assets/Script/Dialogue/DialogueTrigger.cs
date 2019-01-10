using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    

    public void TriggerDialogue()
    {
        DialogueManager x = (DialogueManager) FindObjectOfType(typeof(DialogueManager));
        x.StartDialogue();
    }
}
