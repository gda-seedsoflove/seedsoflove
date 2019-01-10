using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    //object to turn text file into usable dialogue objects
    DialogueParser parser;

    public string dialogue, characterName;
    public int lineNum;
    int pose;
    string position;
    string[] options;
    public bool playerTalking;
    List<Button> buttons = new List<Button>();

    public Text nameText;
    public Text dialogueText;
    public GameObject choiceBox;

    //animator needed to animate dialogue starting and finishing
    public Animator animator;

    //initializes variables, triggers start animation, and starts dialogue
    public void StartDialogue ()
    {
        dialogue = "";
        characterName = "";
        pose = 0;
        position = "L";
        playerTalking = false;

        parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();

        lineNum = 0;

        animator.SetBool("IsOpen", true);

        ShowDialogue();
    }

    //connected to continue button; shows current line, increments, and updates UI
    public void ShowDialogue()
    {
        if(!playerTalking)
        {
            ResetImages();
            ParseLine();
            lineNum++;
        }
        
        UpdateUI();
    }

    //resets images each time a new line is shown
    void ResetImages()
    {
        if (characterName != "")
        {
            GameObject character = GameObject.Find(characterName);
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = null;
        }
    }

    //determines if line is player choice, NPC dialogue, or the end of the scene
    void ParseLine()
    {
        if(parser.GetName(lineNum) == "end")
        {
            EndDialogue();
        }
        else if (parser.GetName(lineNum) != "Player")
        {
            playerTalking = false;
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);
            pose = parser.GetPose(lineNum);
            position = parser.GetPosition(lineNum);
            DisplayImages();
        }
        else
        {
            playerTalking = true;
            characterName = "";
            dialogue = "";
            pose = 0;
            position = "";
            options = parser.GetOptions(lineNum);
            CreateButtons();
        }
    }

    //loads relevant images for current line of dialogue
    void DisplayImages()
    {
        if (characterName != "")
        {
            GameObject character = GameObject.Find(characterName);

            SetSpritePositions(character);

            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = character.GetComponent<Character>().characterPoses[pose];
        }
    }

    //loads image to the correct side for current line
    void SetSpritePositions(GameObject spriteObj)
    {
        if (position == "L")
        {
            spriteObj.transform.position = new Vector3(-6, 0);
        }
        else if (position == "R")
        {
            spriteObj.transform.position = new Vector3(6, 0);
        }
        spriteObj.transform.position = new Vector3(spriteObj.transform.position.x, spriteObj.transform.position.y, 0);
    }

    //creates buttons for player inputs
    void CreateButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            cb.SetText(options[i].Split(':')[0]);
            cb.option = options[i].Split(':')[1];
            cb.box = this;
            b.transform.SetParent(this.transform);
            b.transform.localPosition = new Vector3(0, -25 + (i * 50));
            b.transform.localScale = new Vector3(1, 1, 1);
            buttons.Add(b);
        }
    }

    //removes player choice buttons if player isn't talking, updates name and dialogue text
    void UpdateUI()
    {
        if(!playerTalking)
        {
            ClearButtons();
        }

        nameText.text = characterName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue));
    }

    //removes unneeded buttons
    void ClearButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            print("Clearing buttons");
            Button b = buttons[i];
            buttons.Remove(b);
            Destroy(b.gameObject);
        }
    }

    //animates text so that it appears one letter at a time
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    //plays dialogue end animation
    void EndDialogue()
    {
         animator.SetBool("IsOpen", false);
    }
}
