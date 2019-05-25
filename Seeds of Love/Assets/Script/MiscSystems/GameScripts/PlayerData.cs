using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    public float Mood;  // Value will range from 0 to 1. 1 being max mood

    //public List<string> Choicesmade;

    public Hashtable Choicesmade;

    //[HideInInspector]
    public KeyCode leftkeybind, rightkeybind, spacekeybind, dashkeybind;

    void Awake()
    {
        if(instance == null)
        {
            Choicesmade = new Hashtable();
            instance = this;
            leftkeybind = KeyCode.LeftArrow;
            rightkeybind = KeyCode.RightArrow;
            spacekeybind = KeyCode.Space;
            dashkeybind = KeyCode.UpArrow;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
	// Use this for initialization
	void Start () {
		
	}

    //Sets the keybindings to arrowkeys and space
    public void ArrowPreset()
    {
        leftkeybind = KeyCode.LeftArrow;
        rightkeybind = KeyCode.RightArrow;
        spacekeybind = KeyCode.Space;
        dashkeybind = KeyCode.UpArrow;
    }

    //Sets the keybindings to WASD and space
    public void WASDPreset()
    {
        leftkeybind = KeyCode.A;
        rightkeybind = KeyCode.D;
        spacekeybind = KeyCode.Space;
        dashkeybind = KeyCode.W;
    }

    //Sets the keybindings to JKL and space
    public void JKLPreset()
    {
        leftkeybind = KeyCode.J;
        rightkeybind = KeyCode.L;
        spacekeybind = KeyCode.K;
        dashkeybind = KeyCode.I;
    }


}
