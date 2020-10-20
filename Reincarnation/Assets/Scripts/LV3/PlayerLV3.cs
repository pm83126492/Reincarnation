using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV3 : Player
{
    public GameObject obstacle;
    public Transform Hookpoint, Hookpoint2, Hookpoint3, Hookpoint4;
    public DistanceJoint2D PlayerJoint, HookJoint, HookJoint2, HookJoint3, HookJoint4;
    public bool isSwing,isSwingJump,isSwing2,isSwingJump2;
    public GameObject SwingParent;
    public SwingRotation swingRotation;

    public float SwingSpeed = 5;
    public float SwingJumpSpeed = 2;

    protected override void Movement()
    {
        if ((!isSwing && !isSwingJump)&& (!isSwing2 && !isSwingJump2))
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

            //第一隻手指放掉瞬間
            if (touch.phase == TouchPhase.Began)
            {
                if (!isSwing)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        anim.SetBool("Swing", true);
                        obstacle = hit2.collider.gameObject;
                        transform.position = new Vector3(obstacle.transform.position.x, 5, transform.position.z);
                        if (rigidbody2D.velocity.x > 0)
                        {
                            rigidbody2D.velocity = Vector2.right * SwingSpeed;
                            isSwing = true;
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            rigidbody2D.velocity = Vector2.left * SwingSpeed;
                            isSwing = true;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (isSwing)
                {
                    GetComponent<Rigidbody2D>().gravityScale = 1;

                    if (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.x < 4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(SwingSpeed, SwingJumpSpeed);
                    }
                    else if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(-SwingSpeed, SwingJumpSpeed);
                    }
                    if (obstacle != null)
                    {
                        isSwingJump = true;
                        obstacle.transform.parent = null;
                        obstacle.GetComponent<Rigidbody2D>().isKinematic = false;
                        PlayerJoint.enabled = false;
                        obstacle = null;
                        isSwing = false;
                    }
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    anim.SetBool("Swing", false);
                }
                else if(isSwing2)
                {
                    GetComponent<Rigidbody2D>().gravityScale = 1;
                    if (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.x < 4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(SwingSpeed, SwingJumpSpeed);
                    }
                    else if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(-SwingSpeed, SwingJumpSpeed);
                    }
                    if (obstacle != null)
                    {
                        isSwingJump2 = true;
                        obstacle.transform.parent = null;
                        obstacle.GetComponent<Rigidbody2D>().isKinematic = false;
                        PlayerJoint.enabled = false;
                        obstacle = null;
                        isSwing2 = false;
                    }
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    anim.SetBool("Swing", false);
                }
            }    
        }

        //第二隻手指     
        if (Input.touchCount > 1)
        {
            Touch touch2 = Input.GetTouch(1);
            Touch touch1 = Input.GetTouch(0);

            if(touch1.phase == TouchPhase.Began)
            {
                if (!isSwing2)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        anim.SetBool("Swing", true);
                        obstacle = hit2.collider.gameObject;
                        transform.position = new Vector3(obstacle.transform.position.x, 5, transform.position.z);
                        if (rigidbody2D.velocity.x > 0)
                        {
                            rigidbody2D.velocity = Vector2.right * SwingSpeed;
                            isSwing2 = true;
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            rigidbody2D.velocity = Vector2.left * SwingSpeed;
                            isSwing2 = true;
                        }
                        OneTouchX = OneTouchX2 = 0;
                    }
                }
            }

            if (touch1.phase == TouchPhase.Ended)
            {
                if (isSwing2)
                {
                    GetComponent<Rigidbody2D>().gravityScale = 1;
                    if (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.x < 4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(SwingSpeed, SwingJumpSpeed);
                    }
                    else if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(-SwingSpeed, SwingJumpSpeed);
                    }
                    if (obstacle != null)
                    {
                        isSwingJump2 = true;
                        obstacle.transform.parent = null;
                        obstacle.GetComponent<Rigidbody2D>().isKinematic = false;
                        PlayerJoint.enabled = false;
                        obstacle = null;
                        isSwing2 = false;
                    }
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    anim.SetBool("Swing", false);
                }
            }
            //第二隻手指放掉瞬間
            if (touch2.phase == TouchPhase.Began)
            {
                if (!isSwing)
                {
                    if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                    {
                        anim.SetBool("Swing", true);
                        obstacle = hit2.collider.gameObject;
                        transform.position = new Vector3(obstacle.transform.position.x, 5, transform.position.z);
                        if (rigidbody2D.velocity.x > 0)
                        {
                            rigidbody2D.velocity = Vector2.right * SwingSpeed;
                            isSwing = true;
                        }
                        else if (rigidbody2D.velocity.x < 0)
                        {
                            rigidbody2D.velocity = Vector2.left * SwingSpeed;
                            isSwing = true;
                        }
                    }
                }
            }

            if (touch2.phase == TouchPhase.Ended)
            {
                if (isSwing)
                {
                    GetComponent<Rigidbody2D>().gravityScale = 1;
                    if (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.x < 4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(SwingSpeed, SwingJumpSpeed);
                    }
                    else if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -4 && rigidbody2D.velocity.y > 0)
                    {
                        rigidbody2D.velocity = new Vector2(-SwingSpeed, SwingJumpSpeed);
                    }
                    if (obstacle != null)
                    {
                        isSwingJump = true;
                        obstacle.transform.parent = null;
                        obstacle.GetComponent<Rigidbody2D>().isKinematic = false;
                        PlayerJoint.enabled = false;
                        obstacle = null;
                        isSwing = false;
                    }
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    anim.SetBool("Swing", false);
                }
            }

        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isSwing)
            {
                swingRotation.enabled = true;
                
                if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                {
                    obstacle = hit2.collider.gameObject;
                    SwingParent.transform.parent = obstacle.transform.parent;
                    obstacle.transform.parent = SwingParent.transform;
                    obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    //SwingParent.transform.rotation = new Quaternion(0, 0, gameObject.transform.rotation.z, 0);
                    //obstacle.transform.rotation= new Quaternion(0, 0, SwingParent.transform.rotation.z, 0);
                    //transform.position = new Vector3(obstacle.transform.position.x, 5, transform.position.z);
                    transform.position = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y-5, transform.position.z);
                    if (rigidbody2D.velocity.x > 0)
                    {
                        //anim.SetBool("SwingRight", true);
                        rigidbody2D.velocity = Vector2.right* SwingSpeed;
                        isSwing = true;
                    }
                    else if (rigidbody2D.velocity.x < 0)
                    {
                        //anim.SetBool("SwingLeft", true);
                        rigidbody2D.velocity = Vector2.left * SwingSpeed;
                        isSwing = true;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isSwing)
            {
                swingRotation.enabled = false;
                HookJoint.enabled = true;
                obstacle.transform.parent = SwingParent.transform.parent;
                SwingParent.transform.parent = obstacle.transform;
                GetComponent<Rigidbody2D>().gravityScale = 1;
                if (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.x < 4&& rigidbody2D.velocity.y>0)
                {
                    rigidbody2D.velocity = new Vector2(SwingSpeed, SwingJumpSpeed);
                }
                else if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -4 && rigidbody2D.velocity.y > 0)
                {
                    rigidbody2D.velocity = new Vector2(-SwingSpeed, SwingJumpSpeed);
                }
                if (obstacle != null)
                {
                    isSwingJump = true;
                    //obstacle.transform.parent = null;
                    obstacle.GetComponent<Rigidbody2D>().isKinematic = false;
                    PlayerJoint.enabled = false;
                    //obstacle = null;
                    isSwing = false;
                }
                //anim.SetBool("SwingRight", false);
                //anim.SetBool("SwingLeft", false);
                if (rigidbody2D.velocity.x > 0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
            }
        }

        if (isGround)
        {
            GetComponent<Rigidbody2D>().gravityScale = 3;
            isSwingJump = false;
            isSwingJump2 = false;
        }

        // HookJoint.distance = HookJoint2.distance = HookJoint3.distance = HookJoint4.distance = 2.55f;

        if (isSwing||isSwing2)
        {
            if (isObstacle)
            {
               // obstacle.transform.position = new Vector3(gameObject.transform.position.x, obstacle.transform.position.y, obstacle.transform.position.z);
                if (hit2.collider != null && hit2.collider.gameObject.tag == "Swing")
                {
                    HookJoint.enabled = false;
                    PlayerJoint.connectedAnchor = new Vector2(Hookpoint.position.x, Hookpoint.position.y);
                    Vector3 v = (Hookpoint.position - transform.position).normalized;
                    transform.up = v;
                    Obstacle();
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "Swing2")
                {
                    PlayerJoint.connectedAnchor = new Vector2(Hookpoint2.position.x, Hookpoint2.position.y);
                    Vector3 v = (Hookpoint2.position - transform.position).normalized;
                    transform.up = v;
                    Obstacle();
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "Swing3")
                {
                    PlayerJoint.connectedAnchor = new Vector2(Hookpoint3.position.x, Hookpoint3.position.y);
                    Vector3 v = (Hookpoint3.position - transform.position).normalized;
                    transform.up = v;
                    Obstacle();
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "Swing4")
                {
                    PlayerJoint.connectedAnchor = new Vector2(Hookpoint4.position.x, Hookpoint4.position.y);
                    Vector3 v = (Hookpoint4.position - transform.position).normalized;
                    transform.up = v;
                    Obstacle();
                }
            }
        }
    }

    void Obstacle()
    {
        GetComponent<Rigidbody2D>().gravityScale = 2;
        obstacle.GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerJoint.enabled = true;
    }
}
