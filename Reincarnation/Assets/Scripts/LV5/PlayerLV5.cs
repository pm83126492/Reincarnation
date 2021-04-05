using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV5 : Player
{
    public bool isCanNotAttacked;
    bool isAvoidCD;
    public ParticleSystem ShieldEffect;

    public AudioSource audioSource;
    public AudioClip safeAudio;
    public AudioClip BeAttackAudio;

    public CDImage cdImage;

    protected override void Start()
    {
        SceneSingleton.Instance.SetState(0);
        base.Start();
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();
    }

    protected override void Update()
    {
        base.Update();

        AvoidAttack();
    }

    void AvoidAttack()
    {
        if (useObjButton.Pressed && !cdImage.isStartTimer)
        {
            audioSource.PlayOneShot(safeAudio);
            ShieldEffect.Play();
            isCanNotAttacked = true;
            cdImage.isStartTimer = true;
            StartCoroutine(AvoidStealth());
        }
    }

    IEnumerator AvoidStealth()
    {
        yield return new WaitForSeconds(1.5f);
        isCanNotAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RunnerKingAttack") && !isCanNotAttacked&&RunnerKingController.WinNumber<25)
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                player.GetComponent<Rigidbody2D>().AddForce(-transform.right * 500);
            }
            else if (other.gameObject.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                player.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);
            }
            audioSource.PlayOneShot(BeAttackAudio);
            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
            Invoke("PlayerDie", 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RunnerKingAttack") && !isCanNotAttacked)
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                player.GetComponent<Rigidbody2D>().AddForce(-transform.right * 500);
            }
            else if (collision.gameObject.transform.position.x < transform.position.x)
            {
                transform.rotation = new Quaternion(0, -180, 0, 0);
                player.GetComponent<Rigidbody2D>().AddForce(-transform.right * 500);
            }
            audioSource.PlayOneShot(BeAttackAudio);
            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
            Invoke("PlayerDie", 2f);
        }
    }

    void PlayerDie()
    {
        SceneSingleton.Instance.SetState(2);
    }

}
