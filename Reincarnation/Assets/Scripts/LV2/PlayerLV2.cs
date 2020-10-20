using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV2 : Player
{

    public GameObject organIce;
    //public GameObject obstacle;
    public GameObject OrganCircle;

    public Transform OrganPosition;

    bool isTouchOrgan;
    public bool CanChangeScene;

    protected override void MobileTouch()
    {
        base.MobileTouch();

        //第一隻手指
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);


            //第一隻手指放掉瞬間
            if (touch.phase == TouchPhase.Ended)
            {
                if (obstacle != null)
                {
                    anim.SetBool("Push", false);
                    anim.SetBool("SquatPush", false);
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                    obstacle.GetComponent<FixedJoint2D>().enabled = false;
                    obstacle = null;
                }
                if (organIce != null)
                {
                    anim.enabled = true;
                    anim.SetBool("Roll", false);
                    organIce.GetComponent<Rigidbody2D>().isKinematic = false;
                    isTouchOrgan = false;
                }
            }
            if (touch.phase == TouchPhase.Stationary && OneTouchX == OneTouchX2)
            {
                if (hit2.collider != null && hit2.collider.gameObject.tag == "organ" && isObstacle)
                {
                    isTouchOrgan = true;
                }
            }
        }

        //第二隻手指     
        if (Input.touchCount > 1)
        {
            isTouch2 = true;
            Touch touch2 = Input.GetTouch(1);
            //第二隻手指放掉瞬間
            if (touch2.phase == TouchPhase.Ended)
            {
                if (obstacle != null)
                {
                    anim.SetBool("Push", false);
                    anim.SetBool("SquatPush", false);
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                    obstacle.GetComponent<FixedJoint2D>().enabled = false;
                    obstacle = null;
                }
            }

            if (touch2.phase == TouchPhase.Stationary)
            {
                if (isObstacle)
                {
                    if (hit2.collider != null && hit2.collider.gameObject.tag == "obstacle" && rigidbody2D.velocity.x > 0)
                    {
                        anim.SetBool("Push", true);
                        Obstacle();
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 4;
                        if (rigidbody2D.velocity.x < 0)
                        {
                            anim.SetBool("Push", false);
                            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                            obstacle.GetComponent<FixedJoint2D>().enabled = false;
                            obstacle = null;
                        }
                    }
                    else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleLeft")
                    {
                        anim.SetBool("SquatPush", true);
                        Obstacle();
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 4;
                        if (rigidbody2D.velocity.x < 0)
                        {
                            anim.SetBool("SquatPush", false);
                            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                            obstacle.GetComponent<FixedJoint2D>().enabled = false;
                            obstacle = null;
                        }
                    }
                    else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleRight")
                    {
                        anim.SetBool("SquatPush", true);
                        Obstacle();
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 5;
                        if (rigidbody2D.velocity.x > 0)
                        {
                            anim.SetBool("SquatPush", false);
                            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                            obstacle.GetComponent<FixedJoint2D>().enabled = false;
                            obstacle = null;
                        }
                    }
                }
            }

        }

        if (OneTouchX != OneTouchX2 || TwoTouchX != TwoTouchX2)
        {
            isTouchOrgan = false;
        }

        //判斷起重機是否放掉
        if (!isObstacle || OneTouchX != OneTouchX2 || TwoTouchX != TwoTouchX2)
        {
            organIce.GetComponent<Rigidbody2D>().isKinematic = false;
        }


        if (Input.GetKey(KeyCode.E))
        {
            if (hit2.collider != null && hit2.collider.gameObject.tag == "organ" && isObstacle)
            {
                isTouchOrgan = true;
            }
            if (isObstacle)
            {
                if (hit2.collider != null && hit2.collider.gameObject.tag == "obstacle")
                {
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 5;
                    if (rigidbody2D.velocity.x >= 0)
                    {
                        Obstacle();
                    }
                    anim.SetBool("Push", true);
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleLeft")
                {
                    Obstacle();
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 3;
                    if (rigidbody2D.velocity.x < 0)
                    {
                        anim.SetBool("SquatPush", false);
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                        obstacle.GetComponent<FixedJoint2D>().enabled = false;
                        obstacle = null;
                    }
                    anim.SetBool("SquatPush", true);
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleRight")
                {
                    Obstacle();
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 5;
                    if (rigidbody2D.velocity.x > 0)
                    {
                        anim.SetBool("SquatPush", false);
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                        obstacle.GetComponent<FixedJoint2D>().enabled = false;
                        obstacle = null;
                    }
                    anim.SetBool("SquatPush", true);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (organIce != null)
            {
                anim.enabled = true;
                anim.SetBool("Roll", false);
                organIce.GetComponent<Rigidbody2D>().isKinematic = false;
                isTouchOrgan = false;
            }
            if (obstacle != null)
            {
                anim.SetBool("Push", false);
                anim.SetBool("SquatPush", false);
                obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                obstacle.GetComponent<FixedJoint2D>().enabled = false;
                obstacle = null;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (isTouchOrgan)
        {
            Organ();
        }

        if ((OneTouchX2 > OneTouchX + 25) || (TwoTouchX2 > TwoTouchX + 25) || Input.GetKey(KeyCode.D) || OneTouchX2 + 25 < OneTouchX || TwoTouchX2 + 25 < TwoTouchX || Input.GetKey(KeyCode.A))
        {
            anim.enabled = true;
            anim.SetBool("Roll", false);
        }
    }

    
    //起重冰事件
    void Organ()
    {
        gameObject.transform.position = OrganPosition.position;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (organIce.transform.position.y < 12)
        {
            OrganCircle.transform.Rotate(0, 0, 100 * Time.deltaTime);
        }
        organIce.GetComponent<Rigidbody2D>().isKinematic = true;
        organIce.GetComponent<Rigidbody2D>().velocity = Vector2.up * 1f;
        if (organIce.transform.position.y >= 12)
        {
            organIce.transform.position = new Vector3(organIce.transform.position.x, 12, organIce.transform.position.z);
            anim.enabled = false;
        }
        anim.SetBool("Roll", true);
    }

    public void ReloadScene()
    {
        CanChangeScene = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DieObjects"))
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.08030701f, -0.04518163f);
            GetComponent<BoxCollider2D>().size = new Vector2(1.270004f, 0.08324409f);
            GetComponent<PlayerLV2>().enabled = false;
            anim.SetTrigger("IceSmokeDie");
        }
    }
}
