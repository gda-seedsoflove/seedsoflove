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

    //structure to store the relevant information to each line of dialogue
    struct DialogueLine
    {
        public string name;
        public string content;
        public int pose;
        public string position;
        public string[] options;

        public DialogueLine(string namein, string contentin, int posein, string positionin)
        {
            name = namein;
            content = contentin;
            pose = posein;
            position = positionin;
            options = new string[0];
        }
    }

    //list of lines that makes up the entirety of dialogue for a scene
    List<DialogueLine> lines;

    //loads the file for this scene, initializes the list of lines, and loads the dialogue in
    private void Start()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        string file = "Assets/DialogueText/" + thisScene.name + "_Dialogue.txt";

        lines = new List<DialogueLine>();

        LoadDialogue(file);
    }

    //parses text file for each line of dialogue, turns them into instances of DialogueLine and adds them to the list of lines
    void LoadDialogue(string filename)
    {
        string line;
        StreamReader r = new StreamReader(filename);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] lineData = line.Split(';');
                    DialogueLine lineEntry;
                    if (lineData[0] == "Player")
                    {
                        lineEntry = new DialogueLine(lineData[0], "", 0, "");
                        lineEntry.options = new string[lineData.Length - 1];

                        for (int i = 1; i < lineData.Length; i++)
                        {
                            lineEntry.options[i - 1] = lineData[i];
                        }
                    }
                    else if (lineData[0] == "end")
                    {
                        lineEntry = new DialogueLine("end", "", 0, "");
                    }
                    else
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
}
