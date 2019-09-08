using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//currently, the choice line in the test script isn't being parsed at all. Why?

public class DialogueManager : MonoBehaviour {

    //object to turn text file into usable dialogue objects
    DialogueParser parser;
    PlayerData PlayerData;

    public string dialogue, characterName;
    public int lineNum;
    int pose;
    string position;
    string[] options;
    public int getLineNum;
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

    // - Modified
    public bool inCoroutine;
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
        PlayerData = GameObject.Find("PlayerData").GetComponent<PlayerData>(); //PlayerData.InstanceOf();
        PlayerData.Choicesmade.Add(SceneManager.GetActiveScene().name, PlayerData.Mood);

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
        Debug.Log("Current Script Line: " + lineNum);
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
            string[] commandData = command.Split(':');
            Debug.Log(commandData[0]);
            SceneCommand(commandData);//some stuff to determine which kind of command it is. commandData!
            ParseLine(); //re-parses the line and checks it again
        }
        else if (parser.GetName(lineNum) != "Player") //character is talking without needed player input
        {
            Debug.Log(lineNum);
            Debug.Log(parser.GetName(lineNum));
            playerTalking = false;
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);
            Debug.Log(dialogue);
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
        if (/*characterName != "" && */command!="exit") //if this character is talking, undim
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
        spaceText.text = "(Enter)";
        spaceText.rectTransform.localPosition = new Vector3(spaceText.rectTransform.localPosition.x, -76, spaceText.rectTransform.localPosition.z);

        int xPos = -125;
        int yPos = -127;

        for (int i = 0; i < options.Length; i++)
        {
            GameObject button = Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();

            cb.SetText(options[i].Split(':')[0]);
            cb.option = options[i].Split(':')[1];
            cb.choiceKey = "";
            cb.choiceValue = "";

            cb.box = this;
            
            cb.transform.SetParent(thisCanvas.transform, false);
            RectTransform transform = b.gameObject.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(xPos + (i * 250), yPos);
            buttons.Add(b);
            if(i == 0)
            {
                b.Select();
            }
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
        spaceText.text = "(Space) ->";
        spaceText.rectTransform.localPosition = new Vector3(spaceText.rectTransform.localPosition.x, -52, spaceText.rectTransform.localPosition.z);
    }


    //animates text so that it appears one letter at a time
    /* Now supporting Rich text (RT, for the sake of typing comments)!!!
     * The way this works is by detecting '<' characters in the text files. That opens a new tag for the letter.
     * So when we dump in letters for Unity's text parser, we add those tags for each individual letter.
     * Also, this will no longer yield back to Unity until the function has computed all RT commands.
     */
    IEnumerator TypeSentence (string sentence, string characterName)
    {
        inCoroutine = true;
        int blipWait = 3; //makes blips play only every blipWait-th character.
        dialogueText.text = ""; // base dialouge text.

        // All of these variables have to do with the rich text stuff.
        bool richText = false; // tells whether we're in a richtext command or not.
        bool richTextSignal = true; // tells the rich text thing command thingy whether we're at the start or the end of the a rich text markup.
        string richTextCommandStart = ""; // contains instructions for the rich text's begining
        string richTextCommandEnd = ""; // contains instructions for the rich text's end.
        string tempRichTextCommand = ""; // temporary holds what the text command is.

        foreach (char letter in sentence.ToCharArray())
        {
            // The following three if statements at this level will deal with rich text.
            if (letter == '<') // Checks for opening RT statement
            {
                richText = true;
                Debug.Log("Opened richtext");
                continue;
            }
            if (letter == '>') // Closes and executes RT command.
            {
                richText = false;
                if(richTextSignal == true) // adding the RT opening command
                {
                    if(string.Equals(tempRichTextCommand, "b")) // for bolding
                    {
                        richTextCommandStart += "<b>";
                        richTextCommandEnd = "</b>" + richTextCommandEnd;
                    }
                    else if(string.Equals(tempRichTextCommand, "i"))
                    {
                        richTextCommandStart += "<i>";
                        richTextCommandEnd = "</i>" + richTextCommandEnd;
                    }
                    else if (string.Equals(tempRichTextCommand.Substring(0, 5), "color"))
                    {
                        richTextCommandStart = "<" + tempRichTextCommand + ">";
                        richTextCommandEnd = "</color>" + richTextCommandEnd;
                    }
                }
                else if(richTextSignal == false) // execution of RT closing command
                {
                    if (string.Equals(tempRichTextCommand, "b")) // for bolding
                    {
                        richTextCommandStart = richTextCommandStart.Replace("<b>","");
                        richTextCommandEnd = richTextCommandEnd.Replace("</b>", "");
                    }
                    else if (string.Equals(tempRichTextCommand, "i"))
                    {
                        richTextCommandStart = richTextCommandStart.Replace("<i>", "");
                        richTextCommandEnd = richTextCommandEnd.Replace("</i>", "");
                    }
                    else if (string.Equals(tempRichTextCommand, "color"))
                    {
                        // yeah, bad programming practice. Shoot me. -Eric
                        string theBigCommand = "";
                        for(int i = richTextCommandStart.Length - 1; i > 0; i--)
                        {
                            theBigCommand = richTextCommandStart.Substring(i);
                            if (theBigCommand.Length >= 5 && richTextCommandStart.Substring(i, 5) == "color")
                            {
                                theBigCommand = richTextCommandStart.Substring(i-1);
                                richTextCommandStart = richTextCommandStart.Replace(theBigCommand, "");
                                break;
                            }
                        }
                        richTextCommandEnd = richTextCommandEnd.Replace("</color>", "");
                    }
                }
                tempRichTextCommand = "";
                richTextSignal = true;
                continue;
            }
            if (richText == true) // Stores RT command
            {
                if (letter == '/')
                {
                    richTextSignal = false;
                    Debug.Log("Command is closing");
                }
                else
                {
                    tempRichTextCommand += letter;
                    Debug.Log("Added " + letter + " character to command");
                }
                continue;
            }
            
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
            
            
            if(richText == false)
            {
                dialogueText.text += richTextCommandStart + letter + richTextCommandEnd;
                yield return null;
            }
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
            if (PlayerData.Choicesmade.Contains(commandData[1]))
            {
                //string value = PlayerData.Choicesmade[commandData[1]].ToString();
                GoToHeader("header:" + PlayerData.Choicesmade[commandData[1]].ToString());
            }
            else
            {
                Debug.Log("Choices hasn't been made yet/Wasn't found.");
                ++lineNum;
            }
        }
        else if (commandData[0].Equals("header"))
        {
            //headers do nothing when called. They are simply markers for other commands
        }
        else if (commandData[0].Equals("jumpTo"))
        {
            lineNum = int.Parse(commandData[1]);
        }
        else if (commandData[0].Equals("jumpToHeader"))
        {
            //can we get rid of hard-coded line numbers all together?
            GoToHeader("header:" + commandData[1]);
        }
        //moodJump:moodValue:headerValueOne:headerValueTwo
        //moodJump:SceneName:moodValue:headerValueOne:headerValueTwo
        else if (commandData[0].Equals("moodJump"))
        {
            if(commandData.Length == 4)
            {
                if (PlayerData.Mood >= float.Parse(commandData[1]))
                {
                    GoToHeader("header:" + commandData[2]);
                }
                else
                {
                    GoToHeader("header:" + commandData[3]);
                }
            }
            else
            {
                if((float)PlayerData.Choicesmade[commandData[1]] >= float.Parse(commandData[2]))
                {
                    GoToHeader("header:" + commandData[3]);
                }
                else
                {
                    GoToHeader("header:" + commandData[4]);
                }
            }
        }
        else
        {
            Debug.Log("No such command " + commandData[0] + " at line " + lineNum);
        }
    }

    void GoToHeader(string header)
    {
        while (!string.Equals(parser.GetCommand(lineNum), header))
        {
            //soft parse until you find the exact header.
            //If the header can't be found, the game will crash
            Debug.Log(parser.GetCommand(lineNum) + header);
            ++lineNum;
        }
        ++lineNum;
    }
}
