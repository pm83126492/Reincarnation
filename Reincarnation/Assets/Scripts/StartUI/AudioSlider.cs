using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider slider;
    public static float AudioVoloume;


    void Update()
    {
        AudioVoloume = slider.value;
    }
}
