using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetData : MonoBehaviour {

    // This scipt should be located at the title screen.
    // Functionality: Resets the playerdata's choices made whenever the player enters the title screen
    void Start()
    {
        if (PlayerData.instance != null)
        {
            PlayerData.instance.Choicesmade.Clear();
            PlayerData.instance.SetJukebox(false);
        }
    }

}
