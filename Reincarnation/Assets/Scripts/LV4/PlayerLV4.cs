using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV4 : Player
{
    public GameObject WaterPS;
    public GameObject WaterObject;
    public bool isEnemyAttack;

    private float deltaX, deltaY;
    private float deltaX2, deltaY2;
    public float WaterSpeedX, WaterSpeedY;
    public float WaterSpeedX2, WaterSpeedY2;
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
        if (isInWater && transform.position.y >= -2)
        {
            transform.position = new Vector2(transform.position.x, -2);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isInWater)
        {
            if (Input.touchCount ==1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        deltaX = touch.position.x - transform.position.x;
                        deltaY = touch.position.y - transform.position.y;
                        rigidbody2D.velocity = Vector2.zero;
                        break;

                    case TouchPhase.Moved:
                        WaterSpeedX = (touch.position.x - deltaX) * Time.deltaTime;
                        WaterSpeedY = (touch.position.y - deltaY) * Time.deltaTime;
                        rigidbody2D.velocity = (new Vector2(Mathf.Clamp(WaterSpeedX, -3, 3), Mathf.Clamp(WaterSpeedY, -3, 3)));
                        break;

                    case TouchPhase.Ended:
                        //WaterSpeedX = WaterSpeedY = 0;
                        //rigidbody2D.velocity = Vector2.zero;
                        //rigidbody2D.isKinematic = false;
                        break;
                }
            }
            else if(Input.touchCount == 2)
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
                        //rigidbody2D.velocity = Vector2.zero;
                        //rigidbody2D.isKinematic = false;
                        break;

                }
            }

            else if(Input.touchCount<1&&(WaterSpeedX!=0|| WaterSpeedY!=0))
            {
                WaterSpeedX = WaterSpeedY = 0;
                rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Instantiate(WaterPS, transform.position, transform.rotation);
            StartCoroutine(isInWaterBool());
            isEnemyAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isInWater = false;
            isEnemyAttack = false;
            rigidbody2D.isKinematic = false;
        }
    }

    IEnumerator isInWaterBool()
    {
        yield return new WaitForSeconds(1f);
        isInWater = true;
        rigidbody2D.isKinematic = true;
        //rigidbody2D.gravityScale = 0;
        gameObject.layer = 10;
        rigidbody2D.velocity = Vector2.zero;
    }
}
