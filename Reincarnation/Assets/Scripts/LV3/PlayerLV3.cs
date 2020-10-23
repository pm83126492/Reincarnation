using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV3 : Player
{
    public GameObject obstacle2;
    public bool isSwing, isSwingJump, isSwing2, isSwingJump2;
    public Transform PlayerPoint;
    public GameObject SwingParent;
    public SwingRotation swingRotation;
    public DistanceJoint2D distance01;

    public float SwingSpeed = 5;
    public float SwingJumpSpeed = 2;
    public float SwingSpeedNumber;

    protected override void Movement()
    {
        if (!isSwing)
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

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isSwing)
            {
                if (hit2.collider != null && (hit2.collider.gameObject.tag == "Swing" || hit2.collider.gameObject.tag == "Swing2" || hit2.collider.gameObject.tag == "Swing3" || hit2.collider.gameObject.tag == "Swing4"))
                {
                    obstacle2 = hit2.collider.gameObject;
                    isSwing = true;
                    rigidbody2D.isKinematic = true;
                    gameObject.transform.parent = PlayerPoint.transform;
                    transform.position = PlayerPoint.position;
                    anim.SetBool("Swing", true);
                    obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                    if (rigidbody2D.velocity.x > 0)
                    {
                        obstacle2.GetComponent<Rigidbody2D>().velocity = Vector2.right * SwingSpeed;
                    }
                    //obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isSwing)
            {
               // isSwing = false;
                transform.parent = null;
                rigidbody2D.isKinematic = false;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rigidbody2D.velocity = new Vector2(10, 8);
                anim.SetBool("Swing", false);
            }
        }

        if (isSwing)
        {
            
        }
    }
}
