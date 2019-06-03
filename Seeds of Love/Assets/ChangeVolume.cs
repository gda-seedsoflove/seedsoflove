using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour {

    public Slider mySlider;

    public void OnValueChanged()
    {
        AudioListener.volume = mySlider.value;
    }

}

