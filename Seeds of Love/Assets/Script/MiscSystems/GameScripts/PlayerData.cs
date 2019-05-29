using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    public float Mood;  // Value will range from 0 to 1. 1 being max mood

    //public List<string> Choicesmade;

    public Hashtable Choicesmade;

    //[HideInInspector]
    public KeyCode leftkeybind, rightkeybind, spacekeybind, dashkeybind;

    public string lastscene;

    private bool jukebox;      //If true, the next rhythm game scene will return back to the last scene. 

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
            lastscene = SceneManager.GetActiveScene().name;
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

    public void SetLastScene(string scenename)
    {
        lastscene = scenename;
    }

    public void SetJukebox(bool isjukebox)
    {
        jukebox = isjukebox;
    }

    public bool GetJukebox()
    {
        return jukebox;
    }

}
