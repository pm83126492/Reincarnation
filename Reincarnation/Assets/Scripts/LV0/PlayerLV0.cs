using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV0 : Player
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();
        if (useObjButton.Pressed && isObstacle)
        {
            if (hit2.collider != null && hit2.collider.gameObject.tag == "smallobstacle")
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
        }
        else if (!useObjButton.Pressed && obstacle != null)
        {
            isPushObstacle = false;
            anim.enabled = true;
            anim.SetBool("SquatPush", false);
            anim.SetBool("-SquatPush", false);
            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
            obstacle.GetComponent<FixedJoint2D>().enabled = false;
            obstacle = null;
        }
    }
}
