using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Options_Panel_Spawn : MonoBehaviour
{
    public GameObject newSelect;

    public GameObject optionsPanel;
    public GameObject pausePanel;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(spawnOptions);
    }

    public void spawnOptions()
    {
        if (optionsPanel.activeSelf == false)
        {

            optionsPanel.SetActive(true);
            pausePanel.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newSelect);
        } else
        {
            optionsPanel.SetActive(false);
            pausePanel.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newSelect);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
