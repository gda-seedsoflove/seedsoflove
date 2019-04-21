using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    public float Mood;  // Value will range from 0 to 1. 1 being max mood

    public List<string> Choicesmade;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
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
	

}
