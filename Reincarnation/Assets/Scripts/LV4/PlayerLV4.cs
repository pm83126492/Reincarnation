using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water2DTool;

public class PlayerLV4 : Player
{
    public GameObject WaterPS;
    public GameObject WaterObject;
    public bool isEnemyAttack;
    public  bool isInWater_Slode;
    bool isInWaterSlodeOnce;
    public Water2D_Simulation simulation;

    public PhysicsMaterial2D NoFriction, HaveFriction;

    private float deltaX, deltaY;
    private float deltaX2, deltaY2;
    public float WaterSpeedX, WaterSpeedY;
    public float WaterSpeedX2, WaterSpeedY2;
    public LayerMask WaterLayer;

    public float WaterOffset = 0.6f;
    public float WaterDistance = 1.2f;
    public float WaterWidth = -0.53f;
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
        WaterCheck();

        if (isInWater && transform.position.y >= -2&& !isSlode)
        {
            transform.position = new Vector2(transform.position.x, -2);
        }

        /*if (isObstacle && hit2.collider.gameObject.tag == "Water")
        {
            isInWater_Slode = true;
        }
        else
        {
            isInWater_Slode = false;
        }*/

        if (isObstacle && hit2.collider.gameObject.tag == "OutWater")
        {
            rigidbody2D.isKinematic = false;
            isInWater = false;
            isSlode = true;
            simulation.buoyantForceMode = Water2D_BuoyantForceMode.None;
        }
        else if(isInWater_Slode)
        {
            isInWater = true;
            simulation.buoyantForceMode = Water2D_BuoyantForceMode.PhysicsBased;
            isSlode = false;
        }
        else
        {
            isSlode = false;
        }
    }

    protected override void FixedUpdate()
    {
        if (isInWater)
        {
            if (Input.touchCount ==1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OneTouchX = OneTouchX2 = TwoTouchX = TwoTouchX2 = TwoTouchY = TwoTouchY2 = 0;
                        rigidbody2D.sharedMaterial = NoFriction;
                        rigidbody2D.isKinematic = true;
                        deltaX = touch.position.x - transform.position.x;
                        deltaY = touch.position.y - transform.position.y;
                        //rigidbody2D.velocity = Vector2.zero;
                        break;

                    case TouchPhase.Moved:
                        WaterSpeedX = (touch.position.x - deltaX) * Time.deltaTime;
                        WaterSpeedY = (touch.position.y - deltaY) * Time.deltaTime;
                        rigidbody2D.velocity = (new Vector2(Mathf.Clamp(WaterSpeedX, -3, 3), Mathf.Clamp(WaterSpeedY, -3, 3)));
                        break;

                    case TouchPhase.Ended:
                        WaterSpeedX = WaterSpeedY = 0;
                        rigidbody2D.velocity = Vector2.zero;
                        rigidbody2D.sharedMaterial = HaveFriction;
                        rigidbody2D.isKinematic = false;
                        break;
                }
            }
            /*else if(Input.touchCount == 2)
            {
                Touch touch = Input.GetTouch(1);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        deltaX2 = touch.position.x - transform.position.x;
                        deltaY2 = touch.position.y - transform.position.y;
                        break;

                    case TouchPhase.Moved:
                        WaterSpeedX2 = (touch.position.x - deltaX2) * Time.deltaTime;
                        WaterSpeedY2 = (touch.position.y - deltaY2) * Time.deltaTime;
                        rigidbody2D.velocity = (new Vector2(Mathf.Clamp(WaterSpeedX2, -3, 3), Mathf.Clamp(WaterSpeedY2, -3, 3)));
                        break;
                    case TouchPhase.Ended:
                        WaterSpeedX2 = WaterSpeedY2 = 0;
                        rigidbody2D.velocity = (new Vector2(Mathf.Clamp(WaterSpeedX, -3, 3), Mathf.Clamp(WaterSpeedY, -3, 3)));
                        break;

                }
            }
            */
           /* else if(Input.touchCount<1&&(WaterSpeedX!=0|| WaterSpeedY!=0))
            {
                WaterSpeedX = WaterSpeedY = 0;
                rigidbody2D.velocity = Vector2.zero;
            }*/

            /*if(isObstacle && hit2.collider.gameObject.tag == "OutWater")
            {
                rigidbody2D.isKinematic = false;
            }*/
        }
        else if (isSlode)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        rigidbody2D.sharedMaterial = NoFriction;
                        rigidbody2D.isKinematic = true;
                        //deltaX = touch.position.x - transform.position.x;
                        OneTouchX = touch.position.x;
                        OneTouchX2 = touch.position.x;
                        break;


                    case TouchPhase.Stationary:
                        if (isInWaterSlodeOnce)
                        {
                            OneTouchX = touch.position.x;
                            OneTouchX2 = touch.position.x;
                            isInWaterSlodeOnce = false;
                        }
                        if (OneTouchX <= OneTouchX2)
                        {
                            rigidbody2D.velocity = (new Vector2(3, rigidbody2D.velocity.y));
                        }
                        break;

                    case TouchPhase.Moved:
                        if (!isInWaterSlodeOnce)
                        {
                            OneTouchX2 = touch.position.x;
                            if (OneTouchX < OneTouchX2)
                            {
                                rigidbody2D.velocity = (new Vector2(3, rigidbody2D.velocity.y));
                            }
                            else if (OneTouchX > OneTouchX2)
                            {
                                rigidbody2D.sharedMaterial = HaveFriction;
                                rigidbody2D.velocity = (new Vector2(-3, rigidbody2D.velocity.y));
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        OneTouchX = OneTouchX2 = 0;
                        rigidbody2D.sharedMaterial = HaveFriction;
                        rigidbody2D.isKinematic = false;
                        //WaterSpeedX = WaterSpeedY = 0;
                        rigidbody2D.velocity = Vector2.zero;
                        break;
                }
            }
        }
        else if(!isSlode&&!isInWater)
        {
            base.FixedUpdate();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Instantiate(WaterPS, transform.position, transform.rotation);
            isInWater = true;
            //isInWater_Slode = true;
            //StartCoroutine(isInWaterBool());
            isEnemyAttack = true;
        }

        if (other.gameObject.CompareTag("OutWater"))
        {
            isInWaterSlodeOnce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isInWater = false;
            isInWater_Slode = false;
            isEnemyAttack = false;
           // rigidbody2D.isKinematic = false;
        }

        if (other.gameObject.CompareTag("OutWater"))
        {
            isInWaterSlodeOnce = false;
        }
    }

    IEnumerator isInWaterBool()
    {
        yield return new WaitForSeconds(1f);
        isInWater = true;
        //rigidbody2D.isKinematic = true;
       // gameObject.layer = 10;
       // rigidbody2D.velocity = Vector2.zero;
    }

    private RaycastHit2D Raycast3(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit3 = Physics2D.Raycast(pos + offset, rayDirection, lengh, WaterLayer);
        Color color = hit3 ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit3;
    }

    void WaterCheck()
    {
        RaycastHit2D WaterCheck = Raycast3(new Vector2(WaterWidth, WaterOffset), Vector2.right, WaterDistance);
        if (WaterCheck)
        {
            isInWater_Slode = true;
        }
        else
        {
            isInWater_Slode = false;
        }
    }
}
