using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates an empty array for character sprites to be dragged into character object inspector
public class Character : MonoBehaviour {

    public Sprite[] characterPoses = null;
    public Font personalFont;

    public Font GetFont()
    {
        return personalFont;
    }
}
