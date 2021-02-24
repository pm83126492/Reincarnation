using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioClip[] FootAudio;
    public AudioClip JumpAudio;
    public AudioClip ShineAudio;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return FootAudio[Random.Range(0, FootAudio.Length)];
    }

    public void Jump()
    {
        audioSource.PlayOneShot(JumpAudio);
    }

    public void Shine()
    {
        audioSource.PlayOneShot(ShineAudio);
    }
}
