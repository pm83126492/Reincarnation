using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV3 : Player
{
    public GameObject obstacle2;
    public bool isSwing, isSwingJump, isSwing2, isSwingJump2,isColliderSwing;
    public GameObject PlayerPoint;
    public GameObject SwingParent;
    public SwingRotation swingRotation;
    public DistanceJoint2D distance01;

    public float SwingSpeed = 1;
    public float SwingJumpSpeed = 2;
    public float SwingSpeedNumber;

    protected override void Movement()
    {
        if (!isSwing&&!isSwing2)
        {
            base.Movement();
        }
        
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();
    }

    protected override void Update()
    {
        base.Update();

        if (isGround)
        {
            obstacle2 = null;
        }

       /* if (Input.GetKeyDown(KeyCode.E))
        {

            if (isSwing&&(hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
            {
                obstacle2 = hit2.collider.gameObject;
                isSwing = true;
                isSwingJump = true;
                rigidbody2D.isKinematic = true;
                anim.SetBool("Swing", true);
                obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                if (rigidbody2D.velocity.x >= 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                    transform.localPosition = new Vector3(0,0,0);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                else if (rigidbody2D.velocity.x < 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                    transform.localPosition = new Vector3(0, 0, 0);
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isSwingJump)
            {
                anim.SetBool("Swing", false);
                isSwingJump = false;
                transform.parent = null;
                rigidbody2D.isKinematic = false;
                if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0&& obstacle2.GetComponent<Rigidbody2D>().velocity.y>0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                }
                else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                }
                else if(obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    rigidbody2D.velocity = new Vector2(4.5f, 0);
                }
                else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    rigidbody2D.velocity = new Vector2(-4.5f, 0);
                }
            }
        }*/

        if (isGround)
        {
            anim.SetBool("SwingHandOpen", false);
            isSwing = false;
            isSwing2 = false;
        }


        if (useObjButton.Pressed &&!isSwingJump&& hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
        {
            if (obstacle2 != hit2.collider.gameObject)
            {
                obstacle2 = hit2.collider.gameObject;
                isSwing = true;
                isSwingJump = true;
                rigidbody2D.isKinematic = true;
                anim.SetBool("SwingHandOpen", false);
                anim.SetBool("Swing", true);
                obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                if (rigidbody2D.velocity.x >= 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                    transform.localPosition = new Vector3(0, 0, 0);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                else if (rigidbody2D.velocity.x < 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                    transform.localPosition = new Vector3(0, 0, 0);
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                }
                transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
        }
        else if(!useObjButton.Pressed && isSwingJump)
        {
            anim.SetBool("Swing", false);
            anim.SetBool("SwingHandOpen", true);
            isSwingJump = false;
            transform.parent = null;
            rigidbody2D.isKinematic = false;
            if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
            }
            else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
            }
            else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.velocity = new Vector2(4.5f, 0);
            }
            else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                rigidbody2D.velocity = new Vector2(-4.5f, 0);
            }
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Swing")|| other.gameObject.CompareTag("Swing2") || other.gameObject.CompareTag("Swing3") || other.gameObject.CompareTag("Swing4"))
        {
            isColliderSwing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isColliderSwing = false;
    }
}
