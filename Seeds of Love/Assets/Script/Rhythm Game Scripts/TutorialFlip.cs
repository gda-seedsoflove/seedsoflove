using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlip : MonoBehaviour {

    public List<GameObject> Pages;
    private int CurrentPage;        // 0 is page 1
	// Use this for initialization
	void Start () {
        CurrentPage = 0;
        if (Pages.Count > 0)
        {
            Pages[0].SetActive(true);
            for (int i = 1; i < Pages.Count; i++)
            {
                Pages[i].SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentPage++;
            UpdatePage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentPage--;
            UpdatePage();
        }
	}

    public void UpdatePage()
    {
        if (CurrentPage > Pages.Count - 1 )
        {
            CurrentPage = Pages.Count - 1;
        }
        else if (CurrentPage < 0)
        {
            CurrentPage = 0;
        }

        if (Pages.Count > 0)
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                if (i == CurrentPage)
                {
                    Pages[i].SetActive(true);
                }
                else
                {
                    Pages[i].SetActive(false);
                }
            }
        }

    }
}
