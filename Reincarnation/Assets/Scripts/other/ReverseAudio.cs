using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseAudio : MonoBehaviour
{
    protected Joystick joystick;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.Horizontal > 0)
        {
            audioSource.spread = 0;
        }
        else if (joystick.Horizontal < 0)
        {
            audioSource.spread = 360;
        }
    }
}
