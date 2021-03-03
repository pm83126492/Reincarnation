using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    bool isPlayWaterAudio;
    AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (!isPlayWaterAudio)
            {
                audioSource.PlayOneShot(audioClip[0], 0.5f);
                AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4", 1);
                isPlayWaterAudio = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (isPlayWaterAudio && transform.position.y > -2.5f)
            {
                AudioManager.Instance.CanPausePlaySource(true, true, "UnderWater", "4", 1);
                audioSource.PlayOneShot(audioClip[1], 0.2f);
                isPlayWaterAudio = false;
            }
            else if (!isPlayWaterAudio && transform.position.y < -2.5f)
            {
                audioSource.PlayOneShot(audioClip[0], 0.5f);
                AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4", 1);
                isPlayWaterAudio = true;
            }
        }
    }
}
