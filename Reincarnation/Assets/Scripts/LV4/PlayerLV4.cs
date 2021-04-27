using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water2DTool;
using UnityEngine.Rendering;

public class PlayerLV4 : Player
{
    public GameObject WaterPS;//入水特效
    //public GameObject WaterObject;//水物件
    public GameObject WaterVcam;//入水後鏡頭
    public GameObject Shadow;//角色影子
    public GameObject HeadCam;//罪犯頭鏡頭

    public bool isEnemyAttack;//怪物追擊中
    public bool isBeEnemyAttacked;//被怪物攻擊
    public bool isInWaterLadder;//在水裡繩子上
    bool isSwimming;//游泳中
    public bool isPlayWaterAudio;//播放過入水音效
    bool isBeginSwiming;//開始游泳中
    bool isThrow;//投擲中
    bool isEnterWater;//進入水
    //public bool isWoodGround;//在木頭上
    bool isPlayerTounchHighestLadder;//已經碰到繩子最高點

    public Water2D_Simulation simulation;
    public PhysicsMaterial2D NoFriction, HaveFriction;

    private float deltaX, deltaY;
    private float deltaX2, deltaY2;
    public float WaterSpeedX, WaterSpeedY;
    public float WaterSpeedX2, WaterSpeedY2;
    public float ClimbingSpeed;
    public float WaterOffset = 0.6f;
    public float WaterDistance = 1.2f;
    public float WaterWidth = -0.53f;
    public float angle_Sum;

    public LayerMask WaterLayer;

    public AudioSource audioSource;
    public AudioClip[] audioClip;

    public Vector2 direction;

    public BoxCollider2D HeadCollider;

    public SortingGroup sortingGroup;

    public WaterGhostController waterGhostController;
    public BoxCollider2D SpeedCollider;
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();

        PushMoveWood();//推木頭事件
    }

    protected override void Update()
    {
        base.Update();

        WaterHeightLimit();//在水裡高度限制

        //ClimbLadder();//爬繩子事件

        //WaterCheck();//碰撞入水偵測事件

        //WaterAudioCheck();//碰撞入水聲音偵測事件

        ThrowBaitHeadEvent();//投擲罪犯頭事件
    }

    protected override void FixedUpdate()
    {
        WaterSwimming();//水裡游泳事件

        if (isClimbing)
        {
            anim.SetBool("Swiming", false);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, joystick.Vertical * ClimbingSpeed);
            rigidbody2D.gravityScale = 0;
        }
        else if(!isClimbing&&!isPlayerTounchHighestLadder)
        {
            rigidbody2D.gravityScale = 3;
        }
    }

    //在水裡高度限制
    void WaterHeightLimit()
    {
        if (isInWater && transform.position.y >= -2)
        {
            transform.position = new Vector2(transform.position.x, -2);
        }
    }

    //推木頭事件
    void PushMoveWood()
    {
        if (useObjButton.Pressed && isObstacle && hit2.collider.gameObject.transform.localPosition.x < 60)
        {
            if (hit2.collider != null && hit2.collider.gameObject.tag == "boxGround")
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
        else if ((!useObjButton.Pressed && obstacle != null) || (obstacle != null && hit2.collider.gameObject.tag == "boxGround" && hit2.collider.gameObject.transform.localPosition.x > 60))
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

    //爬繩子事件
    void ClimbLadder()
    {
        if (isObstacle && hit2.collider.gameObject.tag == "Ladder")
        {
            Shadow.SetActive(false);
            if (useObjButton.Pressed && !isInWater)
            {
                anim.SetBool("Climb", true);
                if (rigidbody2D.isKinematic == true)
                {
                    rigidbody2D.isKinematic = false;
                }
                isBeginSwiming = false;
                isClimbing = true;
                rigidbody2D.velocity = Vector2.zero;
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
            else if (useObjButton.Pressed && isInWater)
            {
                isInWaterLadder = isClimbing = true;
                anim.SetBool("SwimingIdle", false);
                anim.SetBool("Swiming", false);
                anim.SetBool("WillSwim", false);
                anim.SetBool("Climb", true);
                WaterVcam.SetActive(false);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rigidbody2D.velocity = Vector2.zero;
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                isInWater = false;
            }
        }
        else if (isClimbing && transform.position.y < -2)
        {
            isInWater = true;
            isClimbing = isSwimming = false;
            anim.SetBool("Climb", false);
        }
        else
        {
            Shadow.SetActive(true);
            isClimbing = false;
            anim.SetBool("Climb", false);
        }
    }

    //水裡游泳事件
    void WaterSwimming()
    {
        if (isInWater)
        {
            WaterVcam.SetActive(true);
            if (joystick.Horizontal != 0)
            {
                rigidbody2D.isKinematic = false;  
                if (!isSwimming)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    anim.SetBool("WillSwim", true);
                    anim.SetBool("SwimingIdle", false);
                    isSwimming = true;
                }

                angle_Sum = Mathf.Atan(-joystick.Horizontal / joystick.Vertical) / (Mathf.PI / 180);
                angle_Sum = joystick.Vertical < 0 ? angle_Sum + 180 : angle_Sum;
                if (float.IsNaN(angle_Sum))
                    angle_Sum = 0;
            }
            else if (joystick.Horizontal == 0)
            {
                rigidbody2D.isKinematic = true;
                rigidbody2D.velocity = Vector2.zero;
                anim.SetBool("WillSwim", false);
                isSwimming = false;
                if (!isBeginSwiming)
                {
                    anim.SetBool("SwimingBeginIdle", true);
                    isBeginSwiming = true;
                }
                else
                {
                    anim.SetBool("SwimingBeginIdle", false);
                    anim.SetBool("SwimingIdle", true);
                }
                anim.SetBool("Swiming", false);
            }

            if (joystick.Horizontal < 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle_Sum);
                rigidbody2D.velocity = new Vector2(200 * Time.deltaTime * joystick.Horizontal, 200 * Time.deltaTime * joystick.Vertical);
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            }
            else if (joystick.Horizontal > 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle_Sum);
                rigidbody2D.velocity = new Vector2(200 * Time.deltaTime * joystick.Horizontal, 200 * Time.deltaTime * joystick.Vertical);
                transform.localScale = new Vector3(1, 1, transform.localScale.z);
            }
        }
        else if (!isInWater)
        {
            base.FixedUpdate();
            isBeginSwiming = false;
        }
    }

    //投擲罪犯頭事件
    void ThrowBaitHeadEvent()
    {
        if (isObstacle && hit2.collider.gameObject.tag == "Bait"&& useObjButton.Pressed)
        {
            if (!isThrow)
            {
                SpeedCollider.enabled = false;
                anim.SetBool("Throw",true);
                HeadCam.SetActive(true);
                isCanMove = false;
                StartCoroutine("CloseHeadCam");
            }
        }
    }

    public void ThrowOver()
    {
        anim.SetBool("Throw", false);
    }

    public void ThrowBait()
    {
        Rigidbody2D rigidbody = GameObject.Find("BaitHead").GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
        rigidbody.gameObject.GetComponent<AudioSource>().Play();
        if (!isThrow)
        {
            rigidbody.AddForce(Vector3.left * 120);
            isThrow = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("HighestLadder")&&isClimbing)
        {
            rigidbody2D.velocity=Vector2.zero;
            anim.SetBool("Climb", false);
            anim.SetBool("Spells", true);
            isPlayerTounchHighestLadder = true;
            isClimbing = isCanMove = false;
        }*/
        if (other.CompareTag("Water") && !isGround)
        {
                if (!isClimbing)
                {
                    isSwimming = false;
                    isInWater = true;
                    isEnemyAttack = true;
                    //Invoke("Drowning", 5f); //溺水事件
                }
                else if (isClimbing)
                {
                    isInWaterLadder = isEnemyAttack = true;
                }

        }

        if (other.CompareTag("WaterGhostSpeed"))
        {
            waterGhostController.speed = 300;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder")&&!isPlayerTounchHighestLadder)
        {
            Shadow.SetActive(false);
            if (isClimbing)
            {
                if (joystick.Vertical > 0)
                {
                    anim.enabled = true;
                    anim.SetBool("Climb", true);
                    anim.SetBool("-Climb", false);
                }
                else if (joystick.Vertical < 0)
                {
                    anim.enabled = true;
                    anim.SetBool("-Climb", true);
                    anim.SetBool("Climb", false);
                }
                else if (joystick.Vertical == 0)
                {
                    anim.enabled = false;
                }
            }

            if (useObjButton.Pressed && isInWater)
            {
                transform.parent = other.gameObject.transform;
                transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
                isInWaterLadder = isClimbing = true;
                anim.SetBool("WillSwim", false);
                anim.SetBool("SwimingIdle", false);
                anim.SetBool("Swiming", false);
                anim.SetBool("Climb", true);
                WaterVcam.SetActive(false);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rigidbody2D.velocity = Vector2.zero;
                isInWater = false;
            }
            else if (useObjButton.Pressed && !isInWater)
            {
               // HeadCollider.enabled = false;
                transform.parent = other.gameObject.transform;
                transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
                if (rigidbody2D.isKinematic == true)
                {
                    rigidbody2D.isKinematic = false;
                }
                isBeginSwiming = false;
                isClimbing = true;
                rigidbody2D.velocity = Vector2.zero;
            }
        }
        /*else if (isClimbing && transform.position.y < -2)
        {
            transform.parent = null;
            isInWater = true;
            isClimbing = isSwimming = false;
            anim.SetBool("Climb", false);
        }*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (isClimbing && transform.position.y < -2)
            {
                transform.parent = null;
                isInWater = true;
                isClimbing = isSwimming = false;
                anim.SetBool("Climb", false);
                anim.SetBool("-Climb", false);
            }
            else
            {
                Shadow.SetActive(true);
                transform.parent = null;
                isClimbing = false;
                anim.SetBool("Climb", false);
                anim.SetBool("-Climb", false);
            }
        }

        if (other.CompareTag("Water"))
        {
            //CancelInvoke("Drowning");
            if (isInWaterLadder)
            {
                isInWaterLadder = isEnemyAttack = false;
            }
            else
            {
               // isInWater = isEnemyAttack = isInWaterLadder = isEnterWater = false;
                isInWater = isEnemyAttack = isInWaterLadder = false;
            }

        }

        /* if (other.CompareTag("HighestLadder"))
         {
             isPlayerTounchHighestLadder = false;
         }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boxGround") && collision.gameObject.GetComponent<WoodGround>().isFallingWater&&!isInWater)
        {
            isWoodGround = true;
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boxGround") && collision.gameObject.GetComponent<WoodGround>().isFallingWater && !isInWater)
        {
            isWoodGround = false;
            transform.parent = null;
        }
    }

    IEnumerator isInWaterBool()
    {
        yield return new WaitForSeconds(1f);
        isCanMove = true;
        isInWater = true;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,90);
    }

    IEnumerator CloseHeadCam()
    {
        yield return new WaitForSeconds(7f);
        isCanMove = true;
        HeadCam.SetActive(false);
    }

    private RaycastHit2D Raycast3(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit3 = Physics2D.Raycast(pos + offset, rayDirection, lengh, WaterLayer);
        Color color = hit3 ? Color.red : Color.yellow;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit3;
    }

    //碰撞入水偵測射線
    private RaycastHit2D Raycast4(Vector2 offset,Vector2 rayDirection,float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit4 = Physics2D.Raycast(pos + offset, rayDirection, lengh, WaterLayer);
        Color color = hit4 ? Color.red : Color.blue;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit4;
    }

    //碰撞入水偵測事件
    void WaterCheck()
    {
        RaycastHit2D waterCheck = Raycast4(new Vector2(WaterWidth, WaterOffset-2), Vector2.right, WaterDistance);
        if (waterCheck && !isEnterWater && !isGround)
        {
            if (!isClimbing)
            {
                isSwimming = false;
                isInWater = true;
                isEnemyAttack = isEnterWater = true;
            }
            else if (isClimbing)
            {
                isInWaterLadder = isEnemyAttack = isEnterWater = true;
            }
        }
        else if (!waterCheck && isEnterWater)
        {
            if (isInWaterLadder)
            {
                isInWaterLadder = false;
            }
            else
            {
                isInWater = isEnemyAttack = isInWaterLadder = isEnterWater = false;
            }

        }
    }

    //碰撞入水聲音偵測事件
    void WaterAudioCheck()
    {
        RaycastHit2D waterAudioCheck = Raycast3(new Vector2(WaterWidth, WaterOffset), Vector2.right, WaterDistance);
        if (waterAudioCheck)
        {
            //Invoke("Drowning", 5f); //溺水事件
            if (!isPlayWaterAudio)
            {
                audioSource.PlayOneShot(audioClip[0], 0.5f);
                AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4");
                isPlayWaterAudio = true;
            }
        }
        else
        {
            CancelInvoke("Drowning");
            if (isPlayWaterAudio && transform.position.y > -2.5f)
            {
                AudioManager.Instance.CanPausePlaySource(true, true, "UnderWater", "4");
                audioSource.PlayOneShot(audioClip[1], 0.2f);
                isPlayWaterAudio = false;
            }
            else if (!isPlayWaterAudio && transform.position.y < -2.5f)
            {
                audioSource.PlayOneShot(audioClip[0], 0.5f);
                AudioManager.Instance.CanPausePlaySource(true, false, "UnderWater", "4");
                isPlayWaterAudio = true;
            }
        }
    }

    public void WillSwim()
    {
        anim.SetBool("WillSwim", false);
        anim.SetBool("Swiming", true);
    }

    //溺水事件
    /*void Drowning()
    {
        if (!isPlayDrownAudio)
        {
            AudioManager.Instance.PlaySource("Drown", 1, "4");
            isPlayDrownAudio = true;
        }
        transform.rotation=Quaternion.Euler(0.0f,0.0f,-90.0f);
        isInWater = isCanMove = isEnemyAttack = false;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = false;
        anim.SetBool("SwimingBeginIdle", false);
        anim.SetBool("SwimingIdle", false);
        anim.SetBool("Swiming", false);
        anim.SetBool("Drowning", true);
    }*/

}
