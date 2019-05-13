using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {
    public string option;
    public DialogueManager box;
    public string choiceKey;
    public string choiceValue;

    PlayerData playerData;

    private void Start()
    {
        box = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>(); //PlayerData.InstanceOf();
    }
    public void SetText(string newText)
    {
        this.GetComponentInChildren<Text>().text = newText;
    }

    public void SetOption(string newOption)
    {
        this.option = newOption;
    }

    public void ParseOption()
    {
        if(!choiceKey.Equals("") && !choiceValue.Equals(""))
        {
            playerData.Choicesmade.Add(choiceKey, choiceValue);
        }

        string command = option.Split(',')[0];
        string commandModifier = option.Split(',')[1];
        box.playerTalking = false;

        if(command == "line")
        {
            box.lineNum = int.Parse(commandModifier);
            box.ShowDialogue();
        }
        else if (command == "scene")
        {
            //Application.LoadLevel("Scene" + commandModifier);
        }
    }
}
