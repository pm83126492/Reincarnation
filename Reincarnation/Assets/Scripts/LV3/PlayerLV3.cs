using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV3 : Player
{
    public GameObject obstacle2;
    public bool isSwing, isSwingJump, isSwing2, isSwingJump2;
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

        //第一隻手指
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //第一隻手指
            if (touch.phase == TouchPhase.Began)
            {
                if (!isSwing)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        obstacle2 = hit2.collider.gameObject;
                        isSwing = true;
                        rigidbody2D.isKinematic = true;
                        anim.SetBool("Swing", true);
                        obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                        if (rigidbody2D.velocity.x >= 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                            transform.localRotation = new Quaternion(0, 0, 0, 0);
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            transform.localRotation = new Quaternion(0, 180, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (isSwing)
                {
                    transform.parent = null;
                    rigidbody2D.isKinematic = false;
                    if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(4.5f, 0);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-4.5f, 0);
                    }
                }
                else if (isSwing2)
                {
                    transform.parent = null;
                    rigidbody2D.isKinematic = false;
                    if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(4.5f, 0);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-4.5f, 0);
                    }
                }
            }
        }
        //第二隻手指     
        if (Input.touchCount > 1)
        {
            Touch touch2 = Input.GetTouch(1);
            Touch touch1 = Input.GetTouch(0);

            if (touch1.phase == TouchPhase.Began)
            {
                if (!isSwing2)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        obstacle2 = hit2.collider.gameObject;
                        isSwing2 = true;
                        rigidbody2D.isKinematic = true;
                        anim.SetBool("Swing", true);
                        obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                        if (rigidbody2D.velocity.x >= 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                            transform.localRotation = new Quaternion(0, 0, 0, 0);
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            transform.localRotation = new Quaternion(0, 180, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                        }
                        OneTouchX = OneTouchX2 = 0;
                    }
                }
            }

            if (touch1.phase == TouchPhase.Ended)
            {
                if (isSwing2)
                {
                    transform.parent = null;
                    rigidbody2D.isKinematic = false;
                    if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(4.5f, 0);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-4.5f, 0);
                    }
                }
            }
            //第二隻手指放掉瞬間
            if (touch2.phase == TouchPhase.Began)
            {
                if (!isSwing)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        obstacle2 = hit2.collider.gameObject;
                        isSwing = true;
                        rigidbody2D.isKinematic = true;
                        anim.SetBool("Swing", true);
                        obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                        if (rigidbody2D.velocity.x >= 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                            transform.localRotation = new Quaternion(0, 0, 0, 0);
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                            transform.localPosition = new Vector3(0, 0, 0);
                            transform.localRotation = new Quaternion(0, 180, 0, 0);
                            obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                        }
                    }
                }
            }

            if (touch2.phase == TouchPhase.Ended)
            {
                if (isSwing)
                {
                    transform.parent = null;
                    rigidbody2D.isKinematic = false;
                    if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rigidbody2D.velocity = new Vector2(4.5f, 0);
                    }
                    else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                    {
                        transform.localRotation = new Quaternion(0, 180, 0, 0);
                        rigidbody2D.velocity = new Vector2(-4.5f, 0);
                    }
                }
            }

        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
            {
                obstacle2 = hit2.collider.gameObject;
                isSwing = true;
                rigidbody2D.isKinematic = true;
                anim.SetBool("Swing", true);
                obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                if (rigidbody2D.velocity.x >= 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(0).gameObject.transform;
                    transform.localPosition = new Vector3(0,0,0);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                    transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                else if (rigidbody2D.velocity.x < 0)
                {
                    gameObject.transform.parent = obstacle2.transform.GetChild(1).gameObject.transform;
                    transform.localPosition = new Vector3(0, 0, 0);
                    transform.localRotation = new Quaternion(0, 180, 0, 0);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * -SwingSpeed;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isSwing)
            {
                transform.parent = null;
                rigidbody2D.isKinematic = false;
                if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 40 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0&& obstacle2.GetComponent<Rigidbody2D>().velocity.y>0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    rigidbody2D.velocity = new Vector2(7f, SwingJumpSpeed);
                }
                else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity > -45 && obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0 && obstacle2.GetComponent<Rigidbody2D>().velocity.y > 0)
                {
                    transform.localRotation = new Quaternion(0, 180, 0, 0);
                    rigidbody2D.velocity = new Vector2(-6f, SwingJumpSpeed);
                }
                else if(obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    rigidbody2D.velocity = new Vector2(4.5f, 0);
                }
                else if (obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
                {
                    transform.localRotation = new Quaternion(0, 180, 0, 0);
                    rigidbody2D.velocity = new Vector2(-4.5f, 0);
                }
            }
        }

        if (isGround)
        {
            anim.SetBool("Swing", false);
            isSwing = false;
            isSwing2 = false;
        }

    }
}
