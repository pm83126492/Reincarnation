using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    protected Joystick joystick;
    protected JumpJoyButton jumpJoyButton;
    protected UseObjButton useObjButton;
    public float horizontal;
    public float Vertical;


    public MeshRenderer PlayerRenderer;
    public PhysicsMaterial2D WallPhysics;
    public Rigidbody2D rigidbody2D;
    protected BoxCollider2D boxCollider2D;
    protected Transform player;
    public Animator anim;
    public float runSpeed = 250f;
    public float JumpForce = 13f;
    public float ObjectsGravity=4.5f;
    private float horizontalMove;

    public float StopSlideTime;//停止滑行時間

    public bool isCanMove;
    public bool isNotStop;
    public bool isNotStop2;
    public bool isInWater;//在水裡
    //public bool isSlode;//在斜坡
    public bool isPushObstacle;//推物件中
    //protected bool isBoxGround;

    public float OneTouchX;
    public float OneTouchX2;

    public float OneTouchY;
    public float OneTouchY2;

    public float TwoTouchX;
    public float TwoTouchX2;

    public float TwoTouchY;
    public float TwoTouchY2;

    public float ThreeTouchX;
    public float ThreeTouchX2;

    public float ThreeTouchY;
    public float ThreeTouchY2;

    public float intervals = 0.1f;//間隔時間
    public float intervals2 = 0.1f;//間隔時間

    public bool jump;
    public bool isJumpButton;
    public bool jump2;
    public bool isJumpButton2;
    public bool jump3;
    public bool isJumpButton3;
    public bool isGround;
    public bool isObstacle;
    public bool isTouch2;
    public bool isTouch3;
    public bool isSlide;
    public bool isColliderEnemy;
    public bool isClimbing;
    public bool isWoodGround;//在木頭上

    protected float SlideTime;

    public float footOffset = 0;
    public float groundDistance = 0.5f;
    public float playerWidth = 0.24f;

    public float forwardOffset = 0.6f;
    public float forwardDistance = 1.2f;
    public float forwardWidth = -0.53f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
   
    public RaycastHit2D hit2;

    public GameObject obstacle;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        jumpJoyButton = FindObjectOfType<JumpJoyButton>();
        useObjButton = FindObjectOfType<UseObjButton>();
        isCanMove = true;
        player = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        Input.multiTouchEnabled = true;
        PlayerRenderer = GetComponent<MeshRenderer>();
       // PlayerRenderer.material.shader = OutlineShader;
    }
    protected virtual void Update()
    {
        horizontal = joystick.Horizontal;
        Vertical = joystick.Vertical;

        //PlayerRenderer.material.shader = OutlineShader;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (isCanMove && !isInWater)
        {
            MobileTouch();//判斷手指滑動狀態
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                anim.SetTrigger("Jump");
                rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.S))
            {
                isSlide = true;
                anim.SetBool("Slide", true);
                boxCollider2D.offset = new Vector2(-0.08030701f, 0.25f);
                boxCollider2D.size = new Vector2(1.270004f, 0.6733987f);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                isSlide = false;
                anim.SetBool("Slide", false);
                boxCollider2D.offset = new Vector2(-0.08030701f, 1.668559f);
                boxCollider2D.size = new Vector2(1.270004f, 3.510725f);
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (isObstacle)
                {
                    if (hit2.collider != null && hit2.collider.gameObject.tag == "obstacle")
                    {
                        Obstacle();
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
                        if (rigidbody2D.velocity.x < 0)
                        {
                            anim.SetBool("Push", false);
                            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                            obstacle.GetComponent<FixedJoint2D>().enabled = false;
                            obstacle = null;
                        }
                        anim.SetBool("Push", true);
                    }
                    else if (hit2.collider != null && hit2.collider.gameObject.tag == "smallobstacle")
                    {
                        anim.SetBool("SquatPush", true);
                        Obstacle();
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = ObjectsGravity;
                        if (rigidbody2D.velocity.x < 0)
                        {
                            anim.SetBool("SquatPush", false);
                            obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                            obstacle.GetComponent<FixedJoint2D>().enabled = false;
                            obstacle = null;
                        }
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
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

        }

        PlayerAnimation();//角色動畫
        GroundCheck();//判斷是否在地面上
        obstacleCheck();//判斷是否碰到障礙物
    }

    protected virtual void FixedUpdate()
    {
        //if (isCanMove)
        if (isCanMove&&!isInWater)
        {
            Movement();//角色移動
        }
    }
    protected virtual void PlayerAnimation()
    {
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigidbody2D.velocity.x));
    }

    protected virtual void MobileTouch()
    {
        if (isGround)
        {
            if (joystick.Vertical < -0.5f)
            {
                isSlide = true;
                SlideTime += Time.deltaTime;
                if (SlideTime >= 1.5f)
                {
                    anim.SetBool("Slide", false);
                    boxCollider2D.offset = new Vector2(-0.08030701f, 1.668559f);
                    boxCollider2D.size = new Vector2(1.270004f, 3.510725f);
                }
                else
                {
                    anim.SetBool("Slide", true);
                    boxCollider2D.offset = new Vector2(-0.08030701f, 0.25f);
                    boxCollider2D.size = new Vector2(1.270004f, 0.6733987f);
                }
            }
            else if (joystick.Vertical > -0.5f)
            {
                SlideTime = 0;
                isSlide = false;
                anim.SetBool("Slide", false);
                boxCollider2D.offset = new Vector2(-0.08030701f, 1.668559f);
                boxCollider2D.size = new Vector2(1.270004f, 3.510725f);
            }
        }
        else
        {
            anim.SetBool("Slide", false);
            boxCollider2D.offset = new Vector2(-0.08030701f, 1.668559f);
            boxCollider2D.size = new Vector2(1.270004f, 3.510725f);
        }

        //判斷是否跳
        if (jumpJoyButton.Pressed&&!jump&&isGround)
        {
            jump = true;
            jumpJoyButton.Pressed = false;
        }
    }
    //移動程式
    protected virtual void Movement()
    {
        //靜止
        if(joystick.Horizontal == 0)
        {
            rigidbody2D.velocity = new Vector2(0 * Time.deltaTime, rigidbody2D.velocity.y);
        }
       
        //右移動
        if((Input.GetKey(KeyCode.D) || joystick.Horizontal > 0) && !isClimbing)
        {
            
            rigidbody2D.velocity = new Vector2(runSpeed * Time.deltaTime, rigidbody2D.velocity.y);
            if (!isPushObstacle&&!isWoodGround)
            {
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (isWoodGround)
            {
                transform.localScale = new Vector3(0.4103743f, transform.localScale.y, transform.localScale.z);
            }
        }
        //左移動
        if ((Input.GetKey(KeyCode.A) || joystick.Horizontal < 0)&&!isClimbing)
        {
            
            rigidbody2D.velocity = new Vector2(-runSpeed * Time.deltaTime, rigidbody2D.velocity.y);
            if (!isPushObstacle && !isWoodGround)
            {
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (isWoodGround)
            {
                transform.localScale = new Vector3(-0.4103743f, transform.localScale.y, transform.localScale.z);
            }
        }
        //跳
        if (jump && isGround)
        {
            anim.SetTrigger("Jump");
            rigidbody2D.velocity = Vector2.up * JumpForce;
            isGround = false;
            jump = false;
        }
    }
   
    //偵測地面射線
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, lengh, groundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit;
    }
    //偵測障礙物射線
    private RaycastHit2D Raycast2(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        hit2 = Physics2D.Raycast(pos + offset, rayDirection, lengh, obstacleLayer);
        Color color = hit2 ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit2;

    }

    //偵測地面
    protected virtual void GroundCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-playerWidth, footOffset), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(playerWidth, footOffset), Vector2.down, groundDistance);
        if (leftCheck || rightCheck)
        {
            isGround = true;
            rigidbody2D.sharedMaterial = null;
        }
        else
        {
            isGround = false;
            rigidbody2D.sharedMaterial = WallPhysics;
        }
    }

    protected virtual void obstacleCheck()
    {
        RaycastHit2D obstacleCheck = Raycast2(new Vector2(forwardWidth, forwardOffset), Vector2.right, forwardDistance);
        if (obstacleCheck)
        {
            isObstacle = true;
        }
        else
        {
            isObstacle = false;
        }
    }


    public void Obstacle()
    {
        if (hit2.collider.gameObject.tag == "obstacle"|| hit2.collider.gameObject.tag == "smallobstacle" || hit2.collider.gameObject.tag == "boxGround")
        {
            obstacle = hit2.collider.gameObject;
        }
        else
        {
            obstacle = hit2.collider.transform.parent.gameObject;
        }
        
        obstacle.GetComponent<FixedJoint2D>().enabled = true;
        obstacle.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
    }

}
