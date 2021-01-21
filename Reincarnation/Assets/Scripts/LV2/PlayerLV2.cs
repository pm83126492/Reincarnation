  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV2 : Player
{
    public GameObject organIce;//冰塊起重機物件
    public GameObject OrganCircle;//冰塊起重機轉盤物件

    public Transform OrganPosition;//冰塊起重機轉盤角色操作位置

    public bool isTouchOrgan;//碰觸起重機轉盤中

    public Rope rope;//轉盤繩索程式

    public GameControllerLV2 gameController;

    public Animator BlackAnim;

    //public bool CanChangeScene;//可以切換場景
    protected override void Start()
    {
        base.Start();
        rigidbody2D.sharedMaterial = WallPhysics;
        ObjectsGravity = 4.5f;
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();

        if (Input.GetKey(KeyCode.E))
        {
            if (hit2.collider != null && hit2.collider.gameObject.tag == "organ" && isObstacle)
            {
                rigidbody2D.velocity = Vector2.zero;
                isCanMove = false;
                isTouchOrgan = true;
                Organ();
            }
            if (isObstacle)
            {
                if (hit2.collider != null && hit2.collider.gameObject.tag == "obstacle")
                {
                    if (joystick.Horizontal > 0)
                    {
                        anim.enabled = true;
                        anim.SetBool("Push", true);
                        anim.SetBool("-Push", false);
                    }
                    else if (joystick.Horizontal < 0)
                    {
                        anim.enabled = true;
                        anim.SetBool("Push", false);
                        anim.SetBool("-Push", true);
                    }
                    else if (joystick.Horizontal == 0)
                    {
                        anim.enabled = false;
                    }
                    Obstacle();
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
                }
                if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleLeft")
                {
                    Obstacle();
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
                    anim.SetBool("SquatPush", true);
                }
                else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleRight")
                {
                    Obstacle();
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
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
                isPushObstacle = false;
                anim.SetBool("Push", false);
                anim.SetBool("SquatPush", false);
                obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                obstacle.GetComponent<FixedJoint2D>().enabled = false;
                //obstacle = null;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        RollUseObjButton();//轉冰塊事件

        PushUseObjButton();//推冰塊事件
    }

    
    //起重冰事件
    void Organ()
    {
        anim.SetBool("Roll", true);
        rope.ChangeRopeBendLimit(0);
        gameObject.transform.position = OrganPosition.position;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (organIce.transform.position.y < 11)
        {
            OrganCircle.transform.Rotate(0, 0, 100 * Time.deltaTime);
        }
        organIce.GetComponent<Rigidbody2D>().isKinematic = true;
        organIce.GetComponent<Rigidbody2D>().velocity = Vector2.up * 1f;
        if (organIce.transform.position.y >= 11f)
        {
            organIce.transform.position = new Vector3(organIce.transform.position.x, 11f, organIce.transform.position.z);
            anim.enabled = false;
        }
    }

    //轉冰塊事件
    void RollUseObjButton()
    {
        if (useObjButton.Pressed && isObstacle && hit2.collider.gameObject.tag == "organ")
        {
            rigidbody2D.velocity = Vector2.zero;
            isCanMove = false;
            isTouchOrgan = true;
            Organ();
        }
        else if (!useObjButton.Pressed && joystick.Horizontal == 0 && isTouchOrgan)
        {
            anim.enabled = true;
            anim.SetBool("Roll", false);
            organIce.GetComponent<Rigidbody2D>().isKinematic = false;
            isCanMove = true;
            isTouchOrgan = false;
        }
        else if (!useObjButton.Pressed && joystick.Horizontal != 0 && isTouchOrgan)
        {
            anim.enabled = true;
            anim.SetFloat("WalkSpeed", Mathf.Abs(rigidbody2D.velocity.x));
            organIce.GetComponent<Rigidbody2D>().isKinematic = false;
            isCanMove = true;
            isTouchOrgan = false;
        }
        else
        {
            anim.SetBool("Roll", false);
        }
    }

    //推冰塊事件
    void PushUseObjButton()
    {
        if (useObjButton.Pressed && isObstacle)
        {
            if (hit2.collider != null && hit2.collider.gameObject.tag == "obstacle")
            {
                isPushObstacle = true;
                if (joystick.Horizontal > 0)
                {
                    anim.enabled = true;
                    anim.SetBool("Push", true);
                    anim.SetBool("-Push", false);
                }
                else if (joystick.Horizontal < 0)
                {
                    anim.enabled = true;
                    anim.SetBool("Push", false);
                    anim.SetBool("-Push", true);
                }
                else if (joystick.Horizontal == 0)
                {
                    anim.enabled = false;
                }
                Obstacle();
                obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
            }
            else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleLeft")
            {
                isPushObstacle = true;
                if (joystick.Horizontal > 0)
                {
                    anim.enabled = true;
                    anim.SetBool("SquatPush", true);
                    anim.SetBool("-SquatPush", false);
                }
                else if (joystick.Horizontal < 0)
                {
                    anim.enabled = true;
                    anim.SetBool("SquatPush", false);
                    anim.SetBool("-SquatPush", true);
                }
                else if (joystick.Horizontal == 0)
                {
                    anim.enabled = false;
                }
                Obstacle();
                obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;

            }
            else if (hit2.collider != null && hit2.collider.gameObject.tag == "SmallobstacleRight")
            {
                isPushObstacle = true;
                if (joystick.Horizontal < 0)
                {
                    anim.enabled = true;
                    anim.SetBool("SquatPush", true);
                    anim.SetBool("-SquatPush", false);
                }
                else if (joystick.Horizontal > 0)
                {
                    anim.enabled = true;
                    anim.SetBool("SquatPush", false);
                    anim.SetBool("-SquatPush", true);
                }
                else if (joystick.Horizontal == 0)
                {
                    anim.enabled = false;
                }
                Obstacle();
                obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
            }
        }
        else if (!useObjButton.Pressed && obstacle != null)
        {
            isPushObstacle = false;
            anim.enabled = true;
            anim.SetBool("Push", false);
            anim.SetBool("SquatPush", false);
            anim.SetBool("-Push", false);
            anim.SetBool("-SquatPush", false);
            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
            obstacle.GetComponent<FixedJoint2D>().enabled = false;
            obstacle = null;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DieObjects"))
        {
            //Physics2D.IgnoreLayerCollision(9, 12);
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.08030701f, -0.04518163f);
            GetComponent<BoxCollider2D>().size = new Vector2(1.270004f, 0.08324409f);
            //GetComponent<PlayerLV2>().enabled = false;
            isCanMove = false;
            rigidbody2D.sharedMaterial = null;
            anim.SetTrigger("IceOrganDie");
            StartCoroutine(IceDie());
        }
    }

    IEnumerator IceDie()
    {
        yield return new WaitForSeconds(3f);
        BlackAnim.SetTrigger("FadeOut");
    }
}
