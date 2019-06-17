using System;
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
    public double timing; // converted timing in seconds
    public int snum;    //Special number defalt 0
}

public class BeatmapReader : MonoBehaviour
{
    // for calculating delay for spawning notes
    public double spawnY;  // y coordinate of note's spawn position
    public double bottomY; // y coordinate of note's hit position
    public double speed;   // note speed in [whatever units unity uses] / second
    [HideInInspector]
    public double delay;  // delay between spawning and hitting the note in seconds

    // objects to read from the beatmap file
    public string filepath;
    private FileInfo file;
    private FileStream beatmap;
    private StreamReader reader;
    private char[] nextByte;
    private char[] buffer;
    public TextAsset beatmapFile;

    // beatmap info
    [HideInInspector]
    public int bpm;
    private int timeSigTop;
    //private int timeSigBot;
    private int subdivisions;
    private int prevTimeSig;
    private int prevSubdiv;
    private int lastUpdate; // number of notes since map data was updated

    // info on the next note, read from this in other classes
    [HideInInspector]
    public NoteData nextNote;
    [HideInInspector]
    public bool songEnd; // whether the song is over

    void Awake()
    {
        // open beatmap file and create a stream to read it
        file = new FileInfo(filepath);
        if(file.Exists || beatmapFile != null)
        {
            //beatmap = file.Open(FileMode.Open, FileAccess.Read);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(beatmapFile.text);
            writer.Flush();
            stream.Position = 0;
            reader = new StreamReader(stream, Encoding.UTF8);

            // initialize char buffers
            nextByte = new char[1];
            buffer = new char[8];
            songEnd = false;

            CalculateDelay();

            // initialize next note struct
            nextNote.timing = 0;
            nextNote.timing -= delay;
            nextNote.measure = 1;
            nextNote.beat = 1;

            GetMapData();
        }
        else Debug.Log("file does not exist");
    }

    // reads next note from the file stream, store in nextNote struct of this class
    public void GetNextNote()
    {
        // if not at the end of file
        if(!reader.EndOfStream)
        {
            // skip until a ( is found (this is always the start of a note)
            char next = SkipUntilChar('(', '=');

            // if a '=' was read, indicates that some map data should change
            if(next == '=')
            {
                UpdateMapData();
            }
            // else a '(' was read, get the next note from file
            else if(!reader.EndOfStream)
            {
                // increment note counter
                lastUpdate++;

                // next byte is the lane
                reader.Read(nextByte, 0, 1);
                nextNote.lane = ConvertCharBuffer(nextByte, 1);
                reader.Read(); // skip the next comma

                // next byte is the note type
                reader.Read(nextByte, 0, 1);
                nextNote.type = nextByte[0];
                reader.Read(); // skip the next comma

                // save timing of prev note
                int prevMes = nextNote.measure;
                int prevBeat = nextNote.beat;

                // read the next 3 bytes, this is measure #
                reader.Read(buffer, 0, 3);
                nextNote.measure = ConvertCharBuffer(buffer, 3);
                reader.Read(); // skip : between measure and beat

                // read the next 2 bytes, this is beat #
                reader.Read(buffer, 0, 2);
                nextNote.beat = ConvertCharBuffer(buffer, 2);

                nextNote.snum = 0;
                SearchSpecialChar(')'); //Searches if the note has an extra part in it. If it does, it will be a special note
 
                ConvertTiming(ref nextNote, prevMes, prevBeat);

            }
            else
            {
                Debug.Log("end of file reached");
                songEnd = true;
            }
        }
        else
        {
            Debug.Log("end of file reached");
            songEnd = true;
        }
    }

    // reads bytes from the stream until a target char is found
    private void SkipUntilChar(char target)
    {
        do
        {
            // if not at end of file
            if(!reader.EndOfStream)
            {
                reader.Read(nextByte, 0, 1);
            }
            else
            {
                Debug.Log("end of file reached");
                break;
            }
        } while(nextByte[0] != target);
    }

    // overloaded version, takes in extra char to check and returns the char found
    private char SkipUntilChar(char target, char target2)
    {
        do
        {
            // if not at end of file
            if(!reader.EndOfStream)
            {
                reader.Read(nextByte, 0, 1);
            }
            else
            {
                Debug.Log("end of file reached");
                break;
            }
        } while(nextByte[0] != target && nextByte[0] != target2);

        return nextByte[0];
    }

    // reads bytes from the stream until a target char is found
    private void SearchSpecialChar(char target)
    {
        do
        {
            // if not at end of file
            if (!reader.EndOfStream)
            {
                reader.Read(nextByte, 0, 1);
                if (nextByte[0] != target)
                {
                    if ((int)nextByte[0] >= 0)
                    {
                        nextNote.snum = nextByte[0];
                    }
                    else
                    {
                        nextNote.snum = 0;
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        } while (nextByte[0] != target);
    }

    // reads the properties of the beatmap from the beginning of the file
    private void GetMapData()
    {
        // if not at end of file
        if(!reader.EndOfStream)
        {
            // skip label
            SkipUntilChar(':');
            reader.Read(); // skip additional space

            // read the next 3 characters, this is bpm
            // Read(buffer, index, count)
            reader.Read(buffer, 0, 3);
            bpm = ConvertCharBuffer(buffer, 3);

            // skip label
            SkipUntilChar(':');
            reader.Read(); // skip additional space

            // read next 3 characters, this is time signature
            reader.Read(nextByte, 0, 1);
            timeSigTop = ConvertCharBuffer(nextByte, 1);
            prevTimeSig = timeSigTop;
            reader.Read(); // skip / in time signature
            reader.Read(nextByte, 0, 1);
            //timeSigBot = ConvertCharBuffer(nextByte, 1);

            // skip label
            SkipUntilChar(':');
            reader.Read(); // skip additional space

            // read next char, this is subdivisions
            reader.Read(nextByte, 0, 1);
            subdivisions = ConvertCharBuffer(nextByte, 1);
            prevSubdiv = subdivisions;

            // initialize notes since last map data update
            lastUpdate = 0;
        }
        else Debug.Log("end of file reached");
    }

    private void UpdateMapData()
    {
        // read the next char
        reader.Read(nextByte, 0, 1);

        // determine which field is being updated based on this char
        // 'B' == updating BPM
        if(nextByte[0] == 'B')
        {
            // skip label
            SkipUntilChar(':');
            reader.Read();

            // read bpm from the next 3 chars, update field
            reader.Read(buffer, 0, 3);
            bpm = ConvertCharBuffer(buffer, 3);
            Debug.Log("BPM changed to " + bpm);
        }
        // 'T' == updating Time Signature
        else if(nextByte[0] == 'T')
        {
            // skip label
            SkipUntilChar(':');
            reader.Read();

            // read time signature from the next char, update field
            reader.Read(nextByte, 0, 1);
            prevTimeSig = timeSigTop;
            timeSigTop = ConvertCharBuffer(nextByte, 1);

            // reset notes since last map update
            lastUpdate = 0;
        }
        // 'S' == updating Subdivisions
        else if(nextByte[0] == 'S')
        {
            // skip label
            SkipUntilChar(':');
            reader.Read();

            // read subdivisions from the next char, update field
            reader.Read(nextByte, 0, 1);
            prevSubdiv = subdivisions;
            subdivisions = ConvertCharBuffer(nextByte, 1);

            // reset notes since last map update
            lastUpdate = 0;
        }
    }

    // convert chars in a buffer to an int value    
    private int ConvertCharBuffer(char[] buffer, int digits)
    {
        // initialize values
        int power = digits - 1;
        int digit = 0, result = 0;
        for (int i = 0; i < digits; i++)
        {
            digit = (int)Char.GetNumericValue(buffer[i]); // convert digit to int
            digit *= (int)Math.Pow(10, power); // multiply digit by its place in the number
            power--;
            result += digit; // add the digit to the total
        }
        return result;
         
    }


    // calculates the delay between when the note spawns and when it should be hit
    private void CalculateDelay()
    {
        // time = distance / velocity
        delay = Math.Abs(bottomY - spawnY);
        delay /= Math.Abs(speed);
    }

    // converts a note's measure:beat timing into seconds
    private void ConvertTiming(ref NoteData n, int prevMes, int prevBeat)
    {
        // convert bpm to beats per second, then seconds per beat
        double bps = bpm / 60.0f; 
        double spb = 1 / bps;
        
        double totalBeat, prevTotalBeat;

        // if the map data was just updated
        if(lastUpdate <= 1)
        {
            // raw beat number = (measures * beats/measure) + (beat / subdivisions)
            // mathematically, the 1st measure should be the "0th" measure, likewise with beat
            totalBeat = (n.measure - 1) * timeSigTop;
            totalBeat += ((double)(n.beat - 1) / (double)subdivisions);

            prevTotalBeat = (prevMes - 1) * prevTimeSig;
            prevTotalBeat += ((double)(prevBeat - 1) / (double)prevSubdiv);
        }
        else
        {
            // raw beat number = (measures * beats/measure) + (beat / subdivisions)
            // mathematically, the 1st measure should be the "0th" measure, likewise with beat
            totalBeat = (n.measure - 1) * timeSigTop;
            totalBeat += ((double)(n.beat - 1) / (double)subdivisions);

            prevTotalBeat = (prevMes - 1) * timeSigTop;
            prevTotalBeat += ((double)(prevBeat - 1) / (double)subdivisions);
        }

        // timing = (raw beat num * sec/beat)
        n.timing += (totalBeat - prevTotalBeat) * spb;

        // Debug.Log(n.timing);
    }

    public float GetBps()
    {
        return bpm / 60.0f;
    }
}
