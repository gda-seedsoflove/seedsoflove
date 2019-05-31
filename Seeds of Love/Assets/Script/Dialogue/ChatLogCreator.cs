using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLogCreator : MonoBehaviour
{
    public CanvasGroup chatLogImage;
    public bool isActive = false;

    // Set chatLogImage to immediately false
    void Awake() {
        chatLogImage.alpha = 0f;
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
        chatLogImage.alpha = 1f;
        isActive = true;
    }

    public void destroyChatLog() {
        chatLogImage.alpha = 0f;
        isActive = false;
    }

}
