using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_Panel_Spawn : MonoBehaviour
{

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
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
