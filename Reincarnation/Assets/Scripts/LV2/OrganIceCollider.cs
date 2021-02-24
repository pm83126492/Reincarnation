using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganIceCollider : MonoBehaviour
{
    public ParticleSystem DustEffect;

    private AudioSource audioSource;

    bool CanPlayAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (CanPlayAudio)
            {
                AudioManager.Instance.PlaySource("IceFalling", 1, "2");
                DustEffect.Play();
            }
            CanPlayAudio = true;
        }
    }

}
