using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorer : MonoBehaviour
{

    float alphaLevel;
    float red;
    float green;
    float blue;
    float alphaSpeed;

    // Use this for initialization
    void Start()
    {
        alphaLevel = 1f;
        alphaSpeed = 0.009f;
        red = 2f;
        green = 1f;
        blue = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        alphaLevel -= alphaSpeed;           //sets the transition speed of the opacity

        /*
         * Sets a temporary color variable to the sprite's color, then modifies the temporary variable's colors,
         * then sets the sprite's color to the temporary variable. (Variable name).(r, g, b, or a) = (any value).
         * Can be applied to any sprite. 1f = 100% and 0f = 0%.
         */

        if (Input.GetMouseButtonDown(0))                        //If left mouse button clicked (Just to test the code)
        {
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = alphaLevel;                                     //modifies opacity
            tmp.r = red;                                            //modifies the red color
            tmp.g = green;                                          //modifies the green color
            tmp.b = blue;                                           //modifies the blue coor
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
