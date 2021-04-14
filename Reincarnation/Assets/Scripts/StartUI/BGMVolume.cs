using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolume : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = BGMSlider.BGMVoloume;
    }
}
