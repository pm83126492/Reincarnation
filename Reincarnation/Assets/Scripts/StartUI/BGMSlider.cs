using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    public Slider slider;
    public static float BGMVoloume;


    void Update()
    {
        BGMVoloume = slider.value;
    }
}
