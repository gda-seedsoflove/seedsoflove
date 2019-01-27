﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Container struct for properties of a note
public struct NoteData
{
    public int lane;   // left to right: 1, 2, 3, or 4
    public char type;  // a = hit, b = lane, 1 = hold begin, 2 = hold end
    public int measure; // raw measure/beat timing
    public int beat;
    public float timing; // converted timing in seconds
}

public class BeatmapReader : MonoBehaviour 
{
	// ***DELETE LATER*** Text objects to print to, for testing/debugging
    public Text lane;
    public Text type;
    public Text time;
    
    // for calculating delay for spawning notes
    public float spawnY;
    public float bottomY;
    public float speed;
    private float delay;
    
    // objects to read from the beatmap file
    public string filepath;
    private FileInfo file;
    private FileStream beatmap;
    private StreamReader reader;
    private char[] nextByte;
    private char[] buffer;
    
    // beatmap info
    private int bpm;
    private int timeSigTop;
    private int timeSigBot;
    private int subdivisions;
    
    // info on the next note, read from this in other classes
    [HideInInspector]
    public NoteData nextNote;
    
    void Start() 
    {
        // open beatmap file and create a stream to read it
        file = new FileInfo(filepath);
        if(file.Exists)
        {
            beatmap = file.Open(FileMode.Open, FileAccess.Read);
            reader = new StreamReader(beatmap, Encoding.UTF8);
        }
        else Debug.Log("file does not exist");
        
        // initialize char buffers
        nextByte = new char[1];
        buffer = new char[8];
        
        StartCoroutine(GetMapData());
	}
	
	void Update() 
    {
        // might not need update loop for now, maybe later to deal with timing but
        // frames might not be accurate timing
	}
    
    // reads the properties of the beatmap from the beginning of the file
    private IEnumerator GetMapData()
    {
        // if not at end of file
        if(!reader.EndOfStream)
        {
            // skip label
            yield return StartCoroutine(SkipUntilChar(':'));
            reader.Read(); // skip additional space
        
            // read the next 3 characters, this is bpm
            // Read(buffer, index, count)
            reader.Read(buffer, 0, 3);
            bpm = ConvertCharBuffer(buffer, 3);
        
            // skip label
            yield return StartCoroutine(SkipUntilChar(':'));
            reader.Read(); // skip additional space
        
            // read next 3 characters, this is time signature
            reader.Read(nextByte, 0, 1);
            timeSigTop = ConvertCharBuffer(nextByte, 1);
            reader.Read(); // skip / in time signature
            reader.Read(nextByte, 0, 1);
            timeSigBot = ConvertCharBuffer(nextByte, 1);
        
            // skip label
            yield return StartCoroutine(SkipUntilChar(':'));
            reader.Read(); // skip additional space
        
            // read next char, this is subdivisions
            reader.Read(nextByte, 0, 1);
            subdivisions = ConvertCharBuffer(nextByte, 1);
        }
        else Debug.Log("end of file reached");
    }
    
    // reads next note from the file stream, store in nextNote struct of this class
    public IEnumerator GetNextNote()
    {   
        // if not at the end of file
        if(!reader.EndOfStream)
        {
            // skip until a ( is found (this is always the start of a note)
            yield return StartCoroutine(SkipUntilChar('('));
        
            if(!reader.EndOfStream)
            {
                // next byte is the lane
                reader.Read(nextByte, 0, 1);
                nextNote.lane = ConvertCharBuffer(nextByte, 1);
                reader.Read(); // skip the next comma
            
                // next byte is the note type
                reader.Read(nextByte, 0, 1);
                nextNote.type = nextByte[0];
                reader.Read(); // skip the next comma
            
                // read the next 3 bytes, this is measure #
                reader.Read(buffer, 0, 3);
                nextNote.measure = ConvertCharBuffer(buffer, 3);
                reader.Read(); // skip : between measure and beat
            
                // read the next 2 bytes, this is beat #
                reader.Read(buffer, 0, 2);
                nextNote.beat = ConvertCharBuffer(buffer, 2);
            }
            else Debug.Log("end of file reached");
        }
        else Debug.Log("end of file reached");
    }
    
    // reads bytes from the stream until a target char is found
    private IEnumerator SkipUntilChar(char target)
    {
        do
        {
            // if not at end of file
            if(!reader.EndOfStream)
            {
                reader.Read(nextByte, 0, 1);
                yield return null;
            }
            else
            {
                Debug.Log("end of file reached");
                break;
            }
        } while(nextByte[0] != target);
    }
    
    // converts a note's measure:beat timing into seconds
    private void ConvertTiming(NoteData n)
    {
        // do this later
    }
    
    // convert chars in a buffer to an int value    
    private int ConvertCharBuffer(char[] buffer, int digits)
    {
        // initialize values
        int power = digits - 1;
        int digit = 0, result = 0;
        
        // for each digit in the buffer
        for(int i = 0; i < digits; i++)
        {
            digit = (int)Char.GetNumericValue(buffer[i]); // convert digit to int
            digit *= (int)Math.Pow(10, power); // multiply digit by its place in the number
            power--;
            result += digit; // add the digit to the total
        }
        return result; 
    }
    
    public void LifeHackButtonCoroutine()
    {
        StartCoroutine(PrintDebugInfo());
    }
    
    public IEnumerator PrintDebugInfo()
    {
        yield return StartCoroutine(GetNextNote());
        lane.text = nextNote.lane.ToString();
        type.text = nextNote.type.ToString();
        time.text = nextNote.measure.ToString() + ":" + nextNote.beat.ToString();
    }
}
