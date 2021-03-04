using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerKingController : MonoBehaviour
{
    private float MaxCountdownTime;
    private float CountdownTime;
    private float AnimoverTime;
    public float BrustSpeed;

    private Animator anim;

    private Rigidbody2D RunnerKingRb;

    public GameObject Fireball;
    public Transform FireballPoint;

    public GameObject AreaGround;
    public Transform[] AreaGroundPoint;

    private Transform PlayerTarget;

    private string currentState;

    public enum State
    {
        IDLE,
        FIREATTACK,
        GROUNDATTACK,
        BURSTATTACK,
    };

    public State RunnerKingState;
    // Start is called before the first frame update
    void Start()
    {
        MaxCountdownTime = Random.Range(1, 3);
        anim = GetComponent<Animator>();
        RunnerKingRb = GetComponent<Rigidbody2D>();
        PlayerTarget = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RunnerKingStateJudgment();
    }

    void RunnerKingStateJudgment()
    {
        CountdownTime += Time.deltaTime;
        switch (RunnerKingState)
        {
            case State.IDLE:
                ChangeAnimationState("idle");
                if (CountdownTime > MaxCountdownTime)
                {
                    int RangeNumber = Random.Range(2, 3);
                    FaceRotation();
                    if (RangeNumber == 1)
                    {
                        RunnerKingState = State.FIREATTACK;
                    }
                    else if (RangeNumber == 2)
                    {
                        RunnerKingState = State.GROUNDATTACK;
                    }
                    else if (RangeNumber == 3)
                    {
                        if (PlayerTarget.position.x > transform.position.x)
                        {
                            BrustSpeed = BrustSpeed * -1;
                        }
                        else if (PlayerTarget.position.x < transform.position.x)
                        {
                            if (BrustSpeed > 0)
                            {
                                BrustSpeed = BrustSpeed * -1;
                            }
                        }
                        RunnerKingState = State.BURSTATTACK;
                    }
                }
                break;
            case State.FIREATTACK:
                ChangeAnimationState("attack");
                IdleState();
                break;
            case State.GROUNDATTACK:
                ChangeAnimationState("special");
                IdleState();
                break;
            case State.BURSTATTACK:            
                ChangeAnimationState("spit");
                IdleState();
                break;
        }
    }

    void IdleState()
    {
        AnimoverTime = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ChangeIdleAnim", AnimoverTime);
    }

    void ChangeIdleAnim()
    {
        RunnerKingState = State.IDLE;
        CountdownTime = 0;
        MaxCountdownTime = Random.Range(1, 4);
        CancelInvoke("ChangeIdleAnim");
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    public void Brust()
    {
        RunnerKingRb.velocity=new Vector2(BrustSpeed, transform.position.y);
    }

    public void Stop()
    {
        RunnerKingRb.velocity = Vector2.zero;
    }

    void FaceRotation()
    {
        if (PlayerTarget.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-0.45f, transform.localScale.y, transform.localScale.z);
        }
        else if (PlayerTarget.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(0.45f, transform.localScale.y, transform.localScale.z);
        }
    }

    public void FireAttack()
    {
        Instantiate(Fireball, FireballPoint.position, FireballPoint.rotation);
    }

    public void GroundAttack()
    {
        int AreaGroundPointRangeNumber= Random.Range(0, 2);
        Instantiate(AreaGround, AreaGroundPoint[AreaGroundPointRangeNumber].position, AreaGroundPoint[AreaGroundPointRangeNumber].rotation);
    }
}
