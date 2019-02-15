using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Canvas thisCanvas;

    public Text nameText;
    public Text dialogueText;
    public GameObject choiceBox;

    //animator needed to animate dialogue starting and finishing
    public Animator animator;

    Queue<GameObject> onScreen;

    //initializes variables, triggers start animation, and starts dialogue
    public void Start()
    {
        dialogue = "";
        characterName = "";
        pose = 0;
        position = "L";
        playerTalking = false;

        parser = GetComponent<DialogueParser>();

        onScreen = new Queue<GameObject>();


        lineNum = 0;

        animator.SetBool("IsOpen", true);

        ShowDialogue();
    }


    //waits for keyboard input so user can advance with click or Space
    void Update()
    {
        if ((Input.GetMouseButtonDown (0) && !playerTalking) || (Input.GetKeyDown("space") && !playerTalking))
        {
            ShowDialogue();
        }
    }

    //shows current line, increments, and updates UI
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
        if (parser.GetContent(lineNum) == "exit")
        {
            var character = GameObject.Find(characterName);
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
        else if(parser.GetContent(lineNum) == "exit")
        {
            //Do Not Dialogue
            playerTalking = false;
            DisplayImages();
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
            options = parser.GetOptions(lineNum);
            CreateButtons();
        }
    }

    //loads relevant images for current line of dialogue
    void DisplayImages()
    {
        if (characterName != "")
        {
            var character = GameObject.Find(characterName);

            if (!onScreen.Contains(character) && onScreen.Count >= 2)
            {
                onScreen.Dequeue().GetComponent<SpriteRenderer>().sprite = null;
                onScreen.Enqueue(character);
                Debug.Log("too corwded");
            }
            else if (!onScreen.Contains(character) && onScreen.Count < 2)
            {
                onScreen.Enqueue(character);
            }
            else
            {
                //nothing, character is already on screen
            }

            if(parser.GetContent(lineNum) == "exit")
            {
                onScreen.Dequeue();
                character.GetComponent<SpriteRenderer>().sprite = null;
                Debug.Log("is run exit command");
            }
            else
            {
                SetSpritePositions(character);

                SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
                currSprite.sprite = character.GetComponent<Character>().characterPoses[pose];

                if (position == "R")
                {
                    currSprite.flipX = true;
                }
                else
                {
                    currSprite.flipX = false;
                }
            }

           

            
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
        int xPos = -150;
        int yPos = -180;

        for (int i = 0; i < options.Length; i++)
        {
            GameObject button = Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            cb.SetText(options[i].Split(':')[0]);
            cb.option = options[i].Split(':')[1];
            cb.box = this;
            
            cb.transform.SetParent(thisCanvas.transform);
            RectTransform transform = b.gameObject.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(xPos + (i * 150), yPos);
            buttons.Add(b);
        }
    }

    //removes player choice buttons if player isn't talking, updates name and dialogue text
    void UpdateUI()
    {
        if(!playerTalking)
        {
            ClearButtons();
            nameText.text = characterName;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogue));
        }

    }

    //removes unneeded buttons
    void ClearButtons()
    {
        while(buttons.Count > 0)
        {
            Button b = buttons[0];
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
