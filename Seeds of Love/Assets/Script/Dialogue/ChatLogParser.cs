using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLogParser : MonoBehaviour
{
    /* TODO:
     * Create a queue/list that holds the dialogue text
     * Set text to the associated queue number
     * If text is greater than text box size
        * Set text's size to be smaller than default
     * Note: Create only a single text obj that holds both name + dialogue
    */

    /* Variables
     * lineNumber = counter for lines (up until 5)
     * nameText = NPC Name
     * dialogueText = NPC's dialogue
     * parser = Object for getting the script of DialogueParser*/
    int lineNum;
    int chatLogCounter;
    public Image chatLogImage;
    public Canvas thisCanvas;
    GameObject[] dialogueTextArray;
    string currentDialogueText;
    int fontSize;
    float textWidth, parentWidth;
    DialogueParser parser;
    DialogueManager manager;
    GameObject dialogueManager;
    Queue<string> chatLogQueue = new Queue<string>();

    public void Start(){
        dialogueManager = GameObject.FindWithTag("DialogueManager");
        if (dialogueManager != null) {
            manager = dialogueManager.GetComponent<DialogueManager>();
        }
        parser = dialogueManager.GetComponent<DialogueParser>();
        currentDialogueText = "";
        lineNum = manager.lineNum;
        chatLogCounter = 0;

        dialogueTextArray = GameObject.FindGameObjectsWithTag("DialogueLine");
    }

    public void Update(){
        if (((Input.GetMouseButtonDown(0) && !manager.playerTalking && manager.inCoroutine) || (Input.GetKeyDown("space") && !manager.playerTalking && manager.inCoroutine))) {
            updateChatLogUI();
        }
    }

    void updateChatLogUI() {
        if (!manager.playerTalking) {
            if (!parser.GetContent(lineNum).Equals("")) {
                currentDialogueText = "<i>" + manager.characterName + "</i>" + ": " + manager.dialogue;
            } else {
                lineNum++;
                currentDialogueText = "<i>" + manager.characterName + "</i>" + ": " + manager.dialogue;
            }
        }
        else {
            currentDialogueText = "<i>" + manager.characterName + "</i>" + ": " + manager.dialogue;
        }

        chatLogQueue.Enqueue(currentDialogueText);
        if(chatLogCounter == 5) {
            chatLogQueue.Dequeue();
        } else {
            chatLogCounter++;
        }

        string[] chatLogArray = chatLogQueue.ToArray();

        int i = 0;
        for(int j = chatLogArray.Length - 1; j >= 0; j--) {
            Text currentText = dialogueTextArray[i].GetComponent<Text>();
            currentText.text = chatLogArray[j];
            i++;
        }
        lineNum++;
    }

    /*bool checkTextWidth() {
        textWidth = LayoutUtility.GetPreferredWidth(dialogueText.rectTransform);
        parentWidth = dialogueText.rectTransform.rect.width;
        Debug.Log(textWidth + " vs " + (parentWidth));
        return (textWidth > parentWidth);
    }*/
}
