using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class DialogueParser : MonoBehaviour
{
    public TextAsset script;
    public TextAsset scriptBad;

    //structure to store the relevant information to each line of dialogue
    struct DialogueLine
    {
        public string name;
        public string content;
        public int pose;
        public string position;
        public string[] options;
        public string command;

        public DialogueLine(string namein, string contentin, int posein, string positionin)
        {
            name = namein;
            content = contentin;
            pose = posein;
            position = positionin;
            options = new string[0];
            command = "";
        }
    }

    //list of lines that makes up the entirety of dialogue for a scene
    List<DialogueLine> lines = new List<DialogueLine>();

    //loads the file for this scene and calls LoadDialogue
    private void Awake()
    {
        //string file = "";
        try
        {
            if (PlayerData.instance.Mood < 0.5f)
            {
                script = scriptBad;
            }
        }
        catch { }

        PlayerData.instance.Choicesmade.Add(SceneManager.GetActiveScene().name, PlayerData.instance.Mood);
        LoadDialogue();
    }

    //parses text file for each line of dialogue, turns them into instances of DialogueLine and adds them to the list of lines
    public void LoadDialogue()
    {
        string line;
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(script.text);
        writer.Flush();
        stream.Position = 0;
        var r = new StreamReader(stream, Encoding.UTF8);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] lineData = line.Split(';');
                    DialogueLine lineEntry;
                    if (lineData[0] == "Player") //player choice line
                    {
                        lineEntry = new DialogueLine(lineData[0], "", 0, "");
                        lineEntry.options = new string[lineData.Length - 1];

                        for (int i = 1; i < lineData.Length; i++)
                        {
                            lineEntry.options[i - 1] = lineData[i];
                        }
                    }
                    else if (lineData[0] == "end" || lineData[0] == "\n")//end of scene
                    {
                        lineEntry = new DialogueLine("end", "", 0, "");
                    }
                    else if(lineData[0]=="##")//non-dialogue command
                    {
                        lineEntry = new DialogueLine(lineData[1], "", 0, "");
                        lineEntry.command = lineData[2];
                    }
                    else //regular line
                    {
                        lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3]);
                    }
                    lines.Add(lineEntry);
                }
            } while (line != null);

            r.Close();
        }
    }

    //gets the position at the line at the int lineNumber
    public string GetPosition(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].position;
        }

        return "";
    }

    //gets the name at the line at the int lineNumber
    public string GetName(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].name;
        }

        return "";
    }

    //gets the content (sentence/s being said) at the line at the int lineNumber
    public string GetContent(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].content;
        }

        return "";
    }

    //gets the pose at the line at the int lineNumber
    public int GetPose(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].pose;
        }

        return 0;
    }

    //gets the dialogue options at the line at the int lineNumber
    //only used if player is speaking
    public string[] GetOptions(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].options;
        }
        return new string[0];
    }

    //returns command for non-dialogue
    public string GetCommand(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].command;
        }
        return "";
    }
}
