using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RunnerKingController : MonoBehaviour
{
    private Bloom bloom;
    public Volume volume;
    public Material FinalLightMaterial;

    public Canvas FinalLightCanvas;

    public float MaxCountdownTime;
    private float CountdownTime;
    public float AnimoverTime;
    public float BrustSpeed;
    float AppearTime;

    bool isAttack;

    public int PointNumber;
    public static int WinNumber;

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

    public GameObject FinalAttack;

    public GameObject MomEffect;
    public Transform MomEffectPoint;
    public SpriteRenderer MomSprite;

    public ObjectPool objectPool;

    private Transform PlayerTarget;

    private string currentState;

    public enum State
    {
        IDLE,
        FIREATTACK,
        GROUNDATTACK,
        TONADOATTACK,
        TESLAATTACK,
        FINAL,
        MOMAPPEAR,
        FINALLIGHT,
    };

    public State RunnerKingState;
    // Start is called before the first frame update
    void Start()
    {
        FinalLightMaterial.color = new Color(2, 2, 2,0);
        MaxCountdownTime =3;
        anim = GetComponent<Animator>();
        RunnerKingRb = GetComponent<Rigidbody2D>();
        PlayerTarget = GameObject.Find("Player").GetComponent<Transform>();
        WinNumber = 0;

        Bloom tmp;
        if (volume.profile.TryGet<Bloom>(out tmp))
        {
            bloom = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RunnerKingStateJudgment();
        if (WinNumber == 20)
        {
            isAttack = false;
            RunnerKingState = State.FINAL;
        }
    }

    void RunnerKingStateJudgment()
    {
        CountdownTime += Time.deltaTime;
        switch (RunnerKingState)
        {
            case State.FINALLIGHT:
                FinalLightCanvas.enabled = true;
                AppearTime += Time.deltaTime;
                bloom.intensity.value = 1.74f;
                FinalLightMaterial.color = new Color(2, 2, 2, AppearTime / 5f);
                break;

            case State.MOMAPPEAR:
                AppearTime += Time.deltaTime;
                MomSprite.color = new Color(1, 1, 1, AppearTime / 1f);
                if (MomSprite.color.a >= 1)
                {
                    RunnerKingState = State.FINALLIGHT;
                    AppearTime = 0;
                }
                break;

            case State.FINAL:
                if (!isAttack)
                {
                    Invoke("FinalAttackEvent", 2f);
                    isAttack = true;
                    WinNumber += 1;
                }
                break;

            case State.IDLE:
                if (CountdownTime >= MaxCountdownTime)
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
                    objectPool.SpawnFromPool("FireBall", FireballPoint.position, FireballPoint.rotation);
                    //Instantiate(Fireball, FireballPoint.position, FireballPoint.rotation);
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
                    //Instantiate(TonadoIce, TonadoIcePoint.position, TonadoIce.transform.rotation);
                    objectPool.SpawnFromPool("Tonadolce", TonadoIcePoint.position, TonadoIce.transform.rotation);
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
        WinNumber += 1;
        RunnerKingState = State.IDLE;
        CountdownTime = 0;
        if (WinNumber <= 5)
        {
            MaxCountdownTime = 2;
        }
        else if (WinNumber > 5&&WinNumber<=10)
        {
            MaxCountdownTime = 1;
        }
        else if (WinNumber > 10 && WinNumber <= 15)
        {
            MaxCountdownTime = 0.5f;
        }
        else if (WinNumber > 15)
        {
            MaxCountdownTime = 0.2f;
        }
        //MaxCountdownTime = Random.Range(1, 4);
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
       // Instantiate(AreaGround, AreaGroundPoint[AreaGroundPointRangeNumber].position, AreaGroundPoint[AreaGroundPointRangeNumber].rotation);
        objectPool.SpawnFromPool("GroundAttack", AreaGroundPoint[AreaGroundPointRangeNumber].position, AreaGroundPoint[AreaGroundPointRangeNumber].rotation);
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
        //Instantiate(TeslaEffect, TeslaPoint[PointNumber].position, transform.rotation);
        objectPool.SpawnFromPool("Tesla", TeslaPoint[PointNumber].position, transform.rotation);
        IdleState();
    }

    void FinalAttackEvent()
    {
        Time.timeScale = 0.5f;
        PlayerTarget.GetComponent<PlayerLV5>().isCanMove = false;
        Invoke("FinalAttackEvent2", 1f);
        FinalAttack.SetActive(true);
    }

    void FinalAttackEvent2()
    {
        Instantiate(MomEffect, MomEffectPoint.position, MomEffect.transform.rotation);
        RunnerKingState = State.MOMAPPEAR;
    }
}
