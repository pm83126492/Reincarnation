using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    protected BoxCollider2D boxCollider2D;
    protected Transform player;
    public Animator anim;
    public float runSpeed = 250f;
    public float JumpForce = 13f;
    private float horizontalMove;

    public bool isNotStop;
    public bool isNotStop2;

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
    void Start()
    {
        player = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        Input.multiTouchEnabled = true;
    }
    protected virtual void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        PlayerAnimation();//角色動畫
        MobileTouch();//判斷手指滑動狀態
        GroundCheck();//判斷是否在地面上
        obstacleCheck();//判斷是否碰到障礙物

        if (Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            anim.SetTrigger("Jump");
            rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("Slide", true);
            boxCollider2D.offset = new Vector2(-0.08030701f, 0.25f);
            boxCollider2D.size = new Vector2(1.270004f, 0.6733987f);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
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
                    obstacle.GetComponent<Rigidbody2D>().gravityScale = 3;
                    if (rigidbody2D.velocity.x < 0)
                    {
                        anim.SetBool("Push", false);
                        obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                        obstacle.GetComponent<FixedJoint2D>().enabled = false;
                        obstacle = null;
                    }
                    anim.SetBool("Push", true);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (obstacle != null)
            {
                anim.SetBool("Push", false);
                obstacle.GetComponent<Rigidbody2D>().gravityScale = 10;
                obstacle.GetComponent<FixedJoint2D>().enabled = false;
                obstacle = null;
            }
        }


        //判斷到地面上停止
        if (isNotStop && isGround)
        {
            OneTouchX = OneTouchX2 = 0;
            isNotStop = false;
        }
        //判斷到地面上停止
        if (isNotStop2 && isGround)
        {
            TwoTouchX = TwoTouchX2= 0;
            if(Input.touchCount == 0)
            {
                OneTouchX = OneTouchX2 = 0;
            }
            isNotStop2 = false;
        }
    }

    void FixedUpdate()
    {
        Movement();//角色移動
    }
    protected virtual void PlayerAnimation()
    {
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigidbody2D.velocity.x));
    }

    protected virtual void MobileTouch()
    {
        //第一隻手指
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //第一隻手指按下瞬間
            if (touch.phase == TouchPhase.Began)
            {
                OneTouchX = touch.position.x;
                OneTouchX2 = touch.position.x;
                OneTouchY = touch.position.y;
                OneTouchY2 = touch.position.y;
            }
            //第一隻手指移動中
            if (touch.phase == TouchPhase.Moved&&!isTouch2)
            {
                OneTouchX2 = touch.position.x;
                if (!isJumpButton)
                {
                    OneTouchY2 = touch.position.y;
                    if (OneTouchY + 50 < OneTouchY2)
                    {
                        isJumpButton = true;
                    }
                }
            }
            //第一隻手指放掉瞬間
            if (touch.phase == TouchPhase.Ended)
            {
                isTouch2 = false;
                intervals = 0.1f;
                if (isGround)
                {
                    OneTouchX = OneTouchX2 = TwoTouchX = TwoTouchX2 = TwoTouchY = TwoTouchY2 = OneTouchY = OneTouchY2 = 0;
                }
                else if (!isGround)
                {
                    isNotStop = true;
                }

                isJumpButton = false;
                if (isJumpButton2)
                {
                    isJumpButton2 = false;
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

        //第二隻手指     
        if (Input.touchCount > 1)
        {
            isTouch2 = true;
            Touch touch2 = Input.GetTouch(1);
            Touch touch1 = Input.GetTouch(0);

            

            //第二隻手指按下瞬間
            if (touch2.phase == TouchPhase.Began)
            {
                TwoTouchX = touch2.position.x;
                TwoTouchX2 = touch2.position.x;
                TwoTouchY = touch2.position.y;
                TwoTouchY2 = touch2.position.y;
            }
            //第二隻手指移動中
            if (touch2.phase == TouchPhase.Moved)
            {
                TwoTouchX2 = touch2.position.x;
                if (!isJumpButton2)
                {
                    TwoTouchY2 = touch2.position.y;
                    if (TwoTouchY + 50 < TwoTouchY2)
                    {
                        isJumpButton2 = true;
                    }
                }
            }
            //第二隻手指放掉瞬間
            if (touch2.phase == TouchPhase.Ended)
            {
                if (isGround)
                {
                    OneTouchX = OneTouchX2 = TwoTouchX = TwoTouchX2 = TwoTouchY = TwoTouchY2 = 0;
                }
                else if (!isGround)
                {
                    isNotStop2 = true;
                }
                isSlide = false;
                anim.SetBool("Slide", false);

                intervals2 = 0.1f;
                isJumpButton2 = false;

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
                }
            }


            if (touch1.phase == TouchPhase.Began)
            {
                ThreeTouchY = touch1.position.y;
                ThreeTouchY2 = touch1.position.y;
                isTouch3 = true;
            }
            if (touch1.phase == TouchPhase.Moved&&isTouch3)
            {
                if (!isJumpButton3)
                {
                    ThreeTouchY2 = touch1.position.y;
                    if (ThreeTouchY + 50 < ThreeTouchY2)
                    {
                        isJumpButton3 = true;
                    }
                }
            }
            if (touch1.phase == TouchPhase.Ended)
            {
                if (isGround)
                {
                    ThreeTouchY = ThreeTouchY2 = 0;
                }
                isJumpButton3 = false;
                isSlide = false;
                isTouch3 = false;
            }
        }

        if ((TwoTouchY > TwoTouchY2 + 50|| ThreeTouchY > ThreeTouchY2 + 50) && rigidbody2D.velocity.x != 0)
        {
            isSlide = true;
        }

        if (isSlide)
        {
            anim.SetBool("Slide", true);
            boxCollider2D.offset = new Vector2(-0.08030701f, 0.25f);
            boxCollider2D.size = new Vector2(1.270004f, 0.6733987f);
        }
        else if(!isSlide)
        {
            anim.SetBool("Slide", false);
            boxCollider2D.offset = new Vector2(-0.08030701f, 1.668559f);
            boxCollider2D.size = new Vector2(1.270004f, 3.510725f);
        }

        if (rigidbody2D.velocity.x == 0)
        {
            isSlide = false;
        }

        //判斷是否跳
        if ((OneTouchY + 50 < OneTouchY2 || TwoTouchY + 50 < TwoTouchY2||ThreeTouchY+50<ThreeTouchY2) && !jump)
        {
            jump = true;
        }
    }
    //移動程式
    protected virtual void Movement()
    {
        //靜止
        if (OneTouchX == 0 || OneTouchX2 == 0 || TwoTouchX == 0 || TwoTouchX2 == 0)
        {
            rigidbody2D.velocity = new Vector2(0 * Time.deltaTime, rigidbody2D.velocity.y);
        }
        //右移動
        if ((OneTouchX2 > OneTouchX + 25) || (TwoTouchX2 > TwoTouchX + 25) || Input.GetKey(KeyCode.D))
        {
            
            rigidbody2D.velocity = new Vector2(runSpeed * Time.deltaTime, rigidbody2D.velocity.y);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        //左移動
        if (OneTouchX2 + 25 < OneTouchX || TwoTouchX2 + 25 < TwoTouchX || Input.GetKey(KeyCode.A))
        {
            
            rigidbody2D.velocity = new Vector2(-runSpeed * Time.deltaTime, rigidbody2D.velocity.y);
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        //跳
        if (jump && isGround)
        {
            anim.SetTrigger("Jump");
            rigidbody2D.velocity = Vector2.up * JumpForce;
            OneTouchY = 0;
            OneTouchY2 = 0;
            TwoTouchY = 0;
            TwoTouchY2 = 0;
            ThreeTouchY = 0;
            ThreeTouchY2 = 0;
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
        }
        else
        {
            isGround = false;
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
        if (hit2.collider.gameObject.tag == "obstacle")
        {
            obstacle = hit2.collider.gameObject;
            obstacle.GetComponent<Rigidbody2D>().gravityScale = 3;
        }
        else
        {
            obstacle = hit2.collider.transform.parent.gameObject;
        }
        
        obstacle.GetComponent<FixedJoint2D>().enabled = true;
        obstacle.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
    }
}
