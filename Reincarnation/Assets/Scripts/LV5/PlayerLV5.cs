using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV5 : Player
{
    bool isCanNotAttacked;
    bool isAvoidCD;
    public Shader OutlineShader;
    public Shader OriginalShader;

    protected override void Start()
    {
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
        if (useObjButton.Pressed && !isAvoidCD)
        {
            PlayerRenderer.material.shader = OutlineShader;
            isCanNotAttacked = true;
            isAvoidCD = true;
            StartCoroutine(AvoidStealth());
        }
    }

    IEnumerator AvoidStealth()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerRenderer.material.shader = OriginalShader;
        isCanNotAttacked = false;
        yield return new WaitForSeconds(1f);
        isAvoidCD = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RunnerKingAttack") && !isCanNotAttacked)
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                player.GetComponent<Rigidbody2D>().AddForce(-transform.right * 500);
            }
            else if (other.gameObject.transform.position.x < transform.position.x)
            {
                transform.rotation = new Quaternion(0, -180, 0, 0);
                player.GetComponent<Rigidbody2D>().AddForce(-transform.right * 500);
            }

            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
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

            isCanMove = false;
            isCanNotAttacked = true;
            anim.SetBool("AttackDie", true);
        }
    }
}
