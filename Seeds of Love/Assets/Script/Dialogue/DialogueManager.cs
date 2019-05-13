using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {

    //object to turn text file into usable dialogue objects
    DialogueParser parser;
    PlayerData playerData;

    public string dialogue, characterName;
    public int lineNum;
    int pose;
    string position;
    string[] options;
    public bool playerTalking;
    string command;

    List<Button> buttons = new List<Button>();
    public Canvas thisCanvas;

    public Text nameText;
    public Text dialogueText;
    public GameObject choiceBox;

    [SerializeField]
    private Text spaceText;

    private bool inTransition;

    //animator needed to animate dialogue starting and finishing
    public Animator animator;

    GameObject stageLeft;
    GameObject stageRight;

    bool inCoroutine;
    private IEnumerator typingRoutine;

    //initializes variables, triggers start animation, and starts dialogue
    public void Start()
    {
        inTransition = false;

        dialogue = "";
        characterName = "";
        pose = 0;
        position = "L";
        playerTalking = false;

        parser = GetComponent<DialogueParser>();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>(); //PlayerData.InstanceOf();

        stageLeft = null;
        stageRight = null;

        lineNum = 0;
        //inCoroutine = false;

        animator.SetBool("IsOpen", true);

        ShowDialogue();
    }


    void Update()
    {
        if(inCoroutine && ((Input.GetMouseButtonDown(0) && !playerTalking) || (Input.GetKeyDown("space") && !playerTalking) && !inTransition)) //shows whole line if click mid line
        {
            StopCoroutine(typingRoutine);
            inCoroutine = false;
            dialogueText.text = "";
            dialogueText.text = dialogue;
        }
        else if(!inCoroutine && ((Input.GetMouseButtonDown (0) && !playerTalking) || (Input.GetKeyDown("space") && !playerTalking) && !inTransition)) //advance line if line is finished
        {
            ShowDialogue();
        }
        /*else if(!inCoroutine && (lineNum>=2 &&((Input.GetMouseButtonDown(1) && !playerTalking) || (Input.GetKeyDown("backspace") && !playerTalking) && !inTransition))) //back to previous line on right click
        {
            lineNum-=2;
            ShowDialogue();
        }*/
        
    }

    //shows current line, increments, and updates UI
    public void ShowDialogue()
    {
        if(!playerTalking)
        {
            ParseLine();
            lineNum++;
        }
        UpdateUI();

        if (command == "exit")
        {
            command = "";
            ShowDialogue();
        }
    }


    //determines if line is player choice, NPC dialogue, or the end of the scene
    void ParseLine()
    {
        if (parser.GetName(lineNum) == "end") //end scene, start scene transition
        {
            EndDialogue();
        }
        else if (parser.GetCommand(lineNum) == "exit") //exit stage left the current character
        {
            playerTalking = false;
            command = parser.GetCommand(lineNum);
            characterName = parser.GetName(lineNum);
            DisplayImages();
        }
        else if (parser.GetName(lineNum) == "Scene")  //this line is a scene command
        {
            playerTalking = false;
            command = parser.GetCommand(lineNum);
            string[] commandData = command.Substring(1, command.Length - 1).Split(':');
            SceneCommand(commandData);//some stuff to determine which kind of command it is. commandData!
        }
        else if (parser.GetName(lineNum) != "Player") //character is talking without needed player input
        {
            playerTalking = false;
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);
            pose = parser.GetPose(lineNum);
            position = parser.GetPosition(lineNum);
            DisplayImages();
        }
        else //player input needed
        {
            playerTalking = true;
            options = parser.GetOptions(lineNum);
            CreateButtons();
        }
    }

    //loads relevant images for current line of dialogue
    void DisplayImages()
    {
        Debug.Log("Character: " + characterName);
        var character = GameObject.Find(characterName);
        if (characterName != "" && command!="exit") //if this character is talking, undim
        {
            SetSpritePositions(character);

            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = character.GetComponent<Character>().characterPoses[pose];

            if(position == "R")
            {
                stageRight = character;
                currSprite.flipX = true;
            }
            else
            {
                stageLeft = character;
                currSprite.flipX = false;
            }
        }
        else if(command == "exit")//if character is exiting... exit
        {
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = null;
        }

        //dimming sprite based on ## command
        if(character != stageLeft && stageLeft != null) //this character isn't on the left and there is a character on the left
        {
            SpriteRenderer currSprite1 = stageLeft.GetComponent<SpriteRenderer>();
            currSprite1.color = new Color(.7f, .7f, .7f, 1f);
            SpriteRenderer currSprite2 = stageRight.GetComponent<SpriteRenderer>();
            currSprite2.color = new Color(1f, 1f, 1f, 1f);
        }
        else if(character != stageRight && stageRight!=null)//this character isn't on the right and there is a character on the right
        {
            SpriteRenderer currSprite1 = stageRight.GetComponent<SpriteRenderer>();
            currSprite1.color = new Color(.7f, .7f, .7f, 1f);
            SpriteRenderer currSprite2 = stageLeft.GetComponent<SpriteRenderer>();
            currSprite2.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (character == stageRight)//this character is on the right
        {
            SpriteRenderer currSprite = stageRight.GetComponent<SpriteRenderer>();
            currSprite.color = new Color(1f, 1f, 1f, 1f);
        }
        else if(character == stageLeft)//this character is on the left
        {
            SpriteRenderer currSprite = stageLeft.GetComponent<SpriteRenderer>();
            currSprite.color = new Color(1f, 1f, 1f, 1f);
        }

    }

    //loads image to the correct side for current dialogue line
    void SetSpritePositions(GameObject spriteObj)
    {
        if (position == "L")
        {
            spriteObj.transform.position = new Vector3(spriteObj.GetComponent<Character>().xpos * -1, spriteObj.GetComponent<Character>().ypos);
        }
        else if (position == "R")
        {
            spriteObj.transform.position = new Vector3(spriteObj.GetComponent<Character>().xpos, spriteObj.GetComponent<Character>().ypos);
        }
        spriteObj.transform.position = new Vector3(spriteObj.transform.position.x, spriteObj.transform.position.y, 0);
    }

    //creates buttons for player inputs
    void CreateButtons()
    {
        spaceText.color = new Color(0, 0, 0, 0);

        int xPos = -125;
        int yPos = -127;

        for (int i = 0; i < options.Length; i++)
        {
            GameObject button = Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            if(options.Length == 2)
            {
                //handles choices that DON'T matter
                cb.SetText(options[i].Split(':')[0]);
                cb.option = options[i].Split(':')[1];
                cb.choiceKey = "";
                cb.choiceValue = "";
            }
            else
            {
                //handles choices that DO matter
                cb.SetText(options[i].Split(':')[0]);
                cb.option = options[i].Split(':')[1];
                cb.choiceKey = options[i].Split(':')[2];
                cb.choiceValue = options[i].Split(':')[3];
            }
            
            cb.box = this;
            
            cb.transform.SetParent(thisCanvas.transform, false);
            RectTransform transform = b.gameObject.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(xPos + (i * 250), yPos);
            buttons.Add(b);
        }
    }

    //removes player choice buttons if player isn't talking, updates name and dialogue text
    void UpdateUI()
    {
        if(!playerTalking && command != "exit" && !inTransition)
        {
            ClearButtons();

            var charObj = GameObject.Find(characterName);
            Character character = charObj.GetComponent<Character>();

            nameText.font = character.GetFont();
            nameText.text = characterName;
            //Change's Maria's name in the character name slot to María with the accented í.
            if (characterName == "Maria")
            {
                nameText.text = "María";
            }

            dialogueText.font = character.GetFont();
            StopAllCoroutines();
            typingRoutine = TypeSentence(dialogue, characterName);
            StartCoroutine(typingRoutine);
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
        spaceText.color = new Color(1, 1, 1, 1);
    }


    //animates text so that it appears one letter at a time
    IEnumerator TypeSentence (string sentence, string characterName)
    {
        inCoroutine = true;
        int blipWait = 3; //makes blips play only every blipWait-th character.
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {

            if(blipWait == 3) //make sure this matches the declaration
            {
                if (characterName.Equals(""))
                {
                    GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>().PlaySound(1, "Note Hit Effect");
                }
                else
                {
                    GameObject.FindWithTag("SoundController").GetComponent<SoundControlScript>().PlaySound(1, characterName + "Blip");
                }
                blipWait = 0;
            }
            else
            {
                ++blipWait;
            }
            
            dialogueText.text += letter;
            yield return null;
        }
        inCoroutine = false;
    }


 

    //plays dialogue end animation
    void EndDialogue()
    {
        inTransition = true;

        //Needs an input of the sceneNumber within script public variables
        animator.SetBool("IsOpen", false);
        SceneFade fadeScreen;
        fadeScreen = GameObject.FindObjectOfType<SceneFade>();
        Debug.Log("Begin EndScene");
        fadeScreen.BeginTransition(fadeScreen.Scenename);
    }

    //runs commands that don't effect a specific character sprite.
    void SceneCommand(string[] commandData)
    {
        //scene command assumes that the command is properly formatted. You'll get an ArrayOutOfBounds if it isn't.
        //"choice" assumes the choice it references has already happened. You'll get an error if it isn't.

        if (commandData[0].Equals("choice"))
        {
            //checks if the [1] value is in the Choicesmade hashtable, then goes to the associated choiceValue header
            if (playerData.Choicesmade.Contains[commandData[1]])
            {
                GoToHeader("##;Scene;{header:" + playerData.Choicesmade[commandData[1]] + "}");
            }
        }
        else if(commandData[0].Equals("header"))
        {
            //headers do nothing when called. They are simply markers for other commands
        }
        else if (commandData[0].Equals("jumpTo"))
        {
            lineNum = int.Parse(commandData[1]);
            
        }
        else
        {
            Debug.Log("No such command " + commandData[0] + " at line " + lineNum);
        }
    }

    //looks for a specific, word-for-word line without printing anything out
    void GoToHeader(string target)
    {
        //If you get EOF, that means that it couldn't find the header
        while (parser.GetContent(lineNum).Equals(target))
        {
            ParseLine(); //will this cause other stuff to happen? If no, we good.
            ++lineNum;
        }
    }
}
