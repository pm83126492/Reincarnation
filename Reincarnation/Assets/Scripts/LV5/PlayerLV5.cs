using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV5 : Player
{
    public bool isCanNotAttacked;
    bool isAvoidCD;

    public int DieNumber;

    public ParticleSystem ShieldEffect;

    public AudioSource audioSource;
    public AudioClip safeAudio;
    public AudioClip BeAttackAudio;

    public RunnerKingController runnerKingController;

    public GameObject LianAppearEffect;

    public CDImage cdImage;
    public ObjectPool objectPool;

    public float SignAppearTime;//符咒顯示時間
    public CanvasGroup SignCanvasGroup;//符咒CanvasGroup
    public GameObject DrawObject, DrawCanvas;//符咒物件 符咒Canvas
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

        DrawToNoDie();

        AvoidAttack();
    }

    void AvoidAttack()
    {
        if (useObjButton.Pressed && !cdImage.isStartTimer&&isCanMove)
        {
            audioSource.PlayOneShot(safeAudio,AudioSlider.AudioVoloume);
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
        if (other.gameObject.CompareTag("RunnerKingAttack") && !isCanNotAttacked&&RunnerKingController.WinNumber<20)
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.AddForce(-transform.right * 500);
            }
            else if (other.gameObject.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.AddForce(transform.right * 500);
            }
            audioSource.PlayOneShot(BeAttackAudio, AudioSlider.AudioVoloume);
            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
            DieNumber += 1;
            if (DieNumber >= 3)
            {
                Invoke("PlayerDie", 2f);
            }
            else
            {
                runnerKingController.isStart = false;
                Invoke("PlayerDie", 5f);
                //Invoke("PlayerRebirth", 3f);
            }
        }
    }

   /* private void OnCollisionEnter2D(Collision2D collision)
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
            audioSource.PlayOneShot(BeAttackAudio, AudioSlider.AudioVoloume);
            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
            Invoke("PlayerDie", 5f);
        }
    }*/

    void PlayerDie()
    {
        DrawObject.SetActive(false);
        DrawCanvas.SetActive(false);
        DieNumber += 1;
        SceneSingleton.Instance.SetState(2);
    }

    void PlayerRebirth()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        runnerKingController.isStart = true;
        runnerKingController.MaxCountdownTime = 3;
        //runnerKingController.RunnerKingState = RunnerKingController.State.IDLE;
        objectPool.SpawnFromPool("AppearEffect", transform.position, ShieldEffect.transform.rotation);
        isCanMove = true;
        isCanNotAttacked = false;
        anim.SetBool("AttackDie", false);
        anim.Play("Idle");
    }

    void DrawToNoDie()
    {
        if (DieNumber == 1)
        {
            SignAppearTime += Time.deltaTime;
            SignCanvasGroup.alpha = SignAppearTime / 1f;
            if (SignCanvasGroup.alpha == 1)
            {
                DrawObject.SetActive(true);
            }

            if (LineCollider.ColliderNumber == 6)
            {
                DieNumber += 1;
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                runnerKingController.isStart = true;
                runnerKingController.MaxCountdownTime = 3;
                //runnerKingController.RunnerKingState = RunnerKingController.State.IDLE;
                objectPool.SpawnFromPool("AppearEffect", transform.position, ShieldEffect.transform.rotation);
                isCanMove = true;
                isCanNotAttacked = false;
                anim.SetBool("AttackDie", false);
                anim.Play("Idle");
                CancelInvoke("PlayerDie");
                DrawCanvas.SetActive(false);
                DrawObject.SetActive(false);
            }
        }
    }
}
