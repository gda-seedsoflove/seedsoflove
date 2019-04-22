using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLogCreator : MonoBehaviour
{
    public GameObject chatLogImage;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()  {
        chatLogImage.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    void Awake() {
        
    }

    public void createOrDestroy() {
        if (isActive) {
            destroyChatLog();
        }
        else {
            createChatLog();
        }
    }

    public void createChatLog() {
        chatLogImage.SetActive(true);
        isActive = true;
    }

    public void destroyChatLog() {
        chatLogImage.SetActive(false);
        isActive = false;
    }

}
