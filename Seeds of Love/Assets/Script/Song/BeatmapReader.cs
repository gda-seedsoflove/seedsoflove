using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// Container struct for properties of a note
public struct NoteData
{
    public int lane;   // left to right: 1, 2, 3, or 4
    public char type;  // (string?) h = hit, l = lane, b = hold begin (?), e = hold end (?)
    public int timing; // (float?)
}

public class BeatmapReader : MonoBehaviour 
{
	// ***DELETE LATER*** Text objects to print to, for testing/debugging
    public Text lane;
    public Text type;
    public Text time;
    
    public string filepath;
    
    private FileInfo file;
    private FileStream beatmap;
    
    void Start() 
    {
        file = new FileInfo(filepath);
        // deal with creating stream when i have more energy
	}
	
	void Update() 
    {
        // might not need update loop for now, maybe later to deal with timing but
        // frames might not be accurate timing
	}
    
    public NoteData NextNote(FileStream s)
    {
        // read...... the note... do it later
    }
    
    public void PrintDebugInfo()
    {
        // later make this get NextNote and adjust text boxes accordingly
        lane.text = "420";
        type.text = "into twinks";
        time.text = "never,";
    }
}
