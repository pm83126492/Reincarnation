﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    bool isPlayWaterAudio;
    bool isPlayDrownAudio;
    AudioSource audioSource;
    public AudioClip[] audioClip;
    PlayerLV4 playerLV4;
    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        playerLV4 = GetComponentInParent<PlayerLV4>();
        //SceneSingleton._Instance.SetState(0);
    }

    private void Update()
    {

        if (playerLV4.isClimbing||playerLV4.isBeEnemyAttacked)
        {
            CancelInvoke("Drowning");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            Invoke("Drowning", 5f);
            if (!isPlayWaterAudio)
            {
                audioSource.PlayOneShot(audioClip[0], AudioSlider.AudioVoloume);
                audioSource.Play();
                //AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4", 1);
                isPlayWaterAudio = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            CancelInvoke("Drowning");
            if (isPlayWaterAudio && transform.position.y > -2.5f)
            {
                audioSource.Stop();
                //AudioManager.Instance.CanPausePlaySource(true, true, "UnderWater", "4", 1);
                audioSource.PlayOneShot(audioClip[1], AudioSlider.AudioVoloume);
                isPlayWaterAudio = false;
            }
            else if (!isPlayWaterAudio && transform.position.y < -2.5f)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(audioClip[0], AudioSlider.AudioVoloume);
                //AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4", 1);
                isPlayWaterAudio = true;
            }
        }
    }

    void Drowning()
    {
        if (!isPlayDrownAudio)
        {
            AudioManager.Instance.PlaySource("Drown", "4");
            playerLV4.anim.SetBool("WillSwim", false);
            isPlayDrownAudio = true;
        }
        if (playerLV4.transform.localScale.x > 0)
        {
            playerLV4.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
        }
        else if (playerLV4.transform.localScale.x < 0)
        {
            playerLV4.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }
        playerLV4.isInWater = playerLV4.isCanMove = playerLV4.isEnemyAttack = false;
        playerLV4.rigidbody2D.velocity = Vector2.zero;
        playerLV4.rigidbody2D.isKinematic = false;
        playerLV4.anim.SetBool("SwimingBeginIdle", false);
        playerLV4.anim.SetBool("SwimingIdle", false);
        playerLV4.anim.SetBool("Swiming", false);
        playerLV4.anim.SetBool("Drowning", true);
        Invoke("ReloadScene", 2f);
    }

    void ReloadScene()
    {
        SceneSingleton._Instance.SetState(2);
    }
}
