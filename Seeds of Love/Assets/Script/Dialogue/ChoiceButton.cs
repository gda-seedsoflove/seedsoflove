using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {
    public string option;
    public DialogueManager box;
    public string choiceKey;
    public string choiceValue;

    PlayerData PlayerData;

    private void Start()
    {
        box = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        PlayerData = GameObject.Find("PlayerData").GetComponent<PlayerData>(); //PlayerData.InstanceOf();
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
        string[] OptionData = option.Split(',');
        string command = OptionData[0];
        string commandModifier = OptionData[1];
        if(OptionData.Length == 4)
        {
            choiceKey = OptionData[2];
            choiceValue = OptionData[3];
            PlayerData.Choicesmade.Add(choiceKey, choiceValue);
            Debug.Log("Choice mattered: " + choiceKey + " " + choiceValue);
        }
        box.playerTalking = false;

        if(command == "line")
        {
            box.lineNum = int.Parse(commandModifier);
            Debug.Log("Going to line " + int.Parse(commandModifier));
            box.ShowDialogue();
        }
        else if (command == "scene")
        {
            //Application.LoadLevel("Scene" + commandModifier);
        }
    }
}
