using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerKingController : MonoBehaviour
{
    private float MaxCountdownTime;
    private float CountdownTime;
    public float AnimoverTime;
    public float BrustSpeed;

    bool isAttack;

    public int PointNumber;

    private Animator anim;

    private Rigidbody2D RunnerKingRb;

    public GameObject Fireball;
    public Transform FireballPoint;

    public GameObject AreaGround;
    public Transform[] AreaGroundPoint;

    public GameObject TonadoIce;
    public Transform TonadoIcePoint;

    public GameObject TeslaEffect;
    public Transform[] TeslaPoint;

    private Transform PlayerTarget;

    private string currentState;

    public enum State
    {
        IDLE,
        FIREATTACK,
        GROUNDATTACK,
        TONADOATTACK,
        TESLAATTACK,
    };

    public State RunnerKingState;
    // Start is called before the first frame update
    void Start()
    {
        MaxCountdownTime =3;
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
                if (CountdownTime > MaxCountdownTime)
                {
                    int RangeNumber = Random.Range(1, 5);
                    isAttack = false;
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
                        RunnerKingState = State.TONADOATTACK;
                    }
                    else if (RangeNumber == 4)
                    {
                        RunnerKingState = State.TESLAATTACK;
                    }
                }
                break;
            case State.FIREATTACK:
                if (!isAttack)
                {
                    Instantiate(Fireball, FireballPoint.position, FireballPoint.rotation);
                    isAttack = true;
                }
                IdleState();
                break;
            case State.GROUNDATTACK:
                if (!isAttack)
                {
                    anim.SetTrigger("RLHand");
                    isAttack = true;
                }
                IdleState();
                break;
            case State.TONADOATTACK:
                if (!isAttack)
                {
                    Instantiate(TonadoIce, TonadoIcePoint.position, TonadoIce.transform.rotation);
                    anim.SetTrigger("RightHand");
                    isAttack = true;
                }
                IdleState();
                break;
            case State.TESLAATTACK:
                if (!isAttack)
                {
                    anim.SetTrigger("LeftHand");
                    isAttack = true;
                }
                break;
        }
    }

    void IdleState()
    {
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

    public void TeslaAtack()
    {
        if (PlayerTarget.position.x > 5)
        {
            PointNumber = 1;
        }
        else if (PlayerTarget.position.x > -5&& PlayerTarget.position.x < 5)
        {
            PointNumber = 0;
        }
        else if (PlayerTarget.position.x < -5)
        {
            PointNumber = 2;
        }
        Instantiate(TeslaEffect, TeslaPoint[PointNumber].position, transform.rotation);
        IdleState();
    }
}
