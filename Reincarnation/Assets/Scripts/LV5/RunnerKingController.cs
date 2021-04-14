using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerKingController : MonoBehaviour
{
    public Image UseObjButton;
    public Image ProtectButton;
    public Canvas ProtectPlane;
    public Canvas ProtectText;

    public GameObject NextButton;

    private Bloom bloom;
    public Volume volume;
    public Material FinalLightMaterial;

    public PlayerLV5 player;
    public EnemyAI Ghost;
    public Animator GhostAnim;
    bool isDieEffect;
    float PlayerIsDieTime;


    public Canvas FinalLightCanvas;

    public float MaxCountdownTime;
    private float CountdownTime;
    public float AnimoverTime;
    public float BrustSpeed;
    float AppearTime;
    float AppearTime2;

    bool isAttack;
    bool isChangeButton;

    public int PointNumber;
    public static int WinNumber;

    private Animator anim;

    private Rigidbody2D RunnerKingRb;

    public GameObject Fireball;
    public Transform FireballPoint;

    public GameObject AreaGround;
    public Transform AreaGroundPoint;

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

    public AudioSource audioSource;
    public AudioClip[] EffectAudio;

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
        FinalLightMaterial.color = new Color(2, 2, 2, 0);
        anim = GetComponent<Animator>();
        RunnerKingRb = GetComponent<Rigidbody2D>();
        PlayerTarget = GameObject.Find("Player").GetComponent<Transform>();
        WinNumber = 0;
        SceneSingleton._Instance.SetState(0);
        Bloom tmp;
        if (volume.profile.TryGet<Bloom>(out tmp))
        {
            bloom = tmp;
        }
        if (!LV5Introdution.isNotOnce)
        {
            MaxCountdownTime = 12;
            StartCoroutine("ChangeButton");
        }
        else
        {
            NextButton.SetActive(true);
            UseObjButton.gameObject.SetActive(false);
            ProtectButton.gameObject.SetActive(true);
            ProtectButton.color = new Color(1, 1, 1, 0.7f);
            MaxCountdownTime = 3;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        RunnerKingStateJudgment();
        if (WinNumber == 20)
        {
            CancelInvoke("ChangeIdleAnim");
            isAttack = false;
            RunnerKingState = State.FINAL;
        }

        if (isChangeButton)
        {
            AppearTime += Time.deltaTime;
            if (UseObjButton.color.a > 0)
            {
                UseObjButton.color = new Color(1, 1, 1, 0.7f - AppearTime / 3f);
            }
            else
            {
                AppearTime2 += Time.deltaTime;
                ProtectButton.color= new Color(1, 1, 1, AppearTime2 / 3f);
                if (ProtectButton.color.a >= 0.7f)
                {
                    StartCoroutine("ChangeButton2");
                    isChangeButton = false;
                }
            }
        }
    }

    void RunnerKingStateJudgment()
    {
        CountdownTime += Time.deltaTime;
        switch (RunnerKingState)
        {
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
                        RunnerKingState = State.TESLAATTACK;
                    }
                    else if (RangeNumber == 4)
                    {
                        RunnerKingState = State.TONADOATTACK;
                    }
                }
                break;
            case State.FIREATTACK:
                if (!isAttack)
                {
                    objectPool.SpawnFromPool("FireBall", FireballPoint.position, FireballPoint.rotation);
                    audioSource.PlayOneShot(EffectAudio[0], AudioSlider.AudioVoloume);
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
                    audioSource.PlayOneShot(EffectAudio[2], AudioSlider.AudioVoloume);
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

            case State.FINAL:
                if (!isAttack)
                {
                    player.isCanMove = false;
                    Ghost.gameObject.SetActive(true);             
                    isAttack = true;
                    WinNumber += 1;
                }

                float EnemyToPlayerDistance = Vector2.Distance(Ghost.transform.position, player.transform.position);
                if (EnemyToPlayerDistance <= 4.5f)
                {
                    PlayerIsDieTime += Time.deltaTime;
                    Ghost.enabled = false;
                    GhostAnim.SetTrigger("TongueAttack");
                    if (PlayerIsDieTime >= 1f)
                    {
                        if (!isDieEffect)
                        {
                            //audioSource.PlayOneShot(ChokingAudio);
                            AudioManager.Instance.PlaySource("PlayerScream", "other");
                            player.anim.SetBool("GhostWhiteAttack", true);
                            isDieEffect = true;
                            Invoke("FinalAttackEvent", 2f);
                        }
                    }
                }
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

            case State.FINALLIGHT:
                FinalLightCanvas.enabled = true;
                AppearTime += Time.deltaTime;
                //bloom.intensity.value = 1.74f;
                FinalLightMaterial.color = new Color(2, 2, 2, AppearTime / 5f);
                if (FinalLightMaterial.color.a> 0.5f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    public void GroundAttack()
    {
        int AreaGroundPointRangeNumber= Random.Range(0, 2);
        audioSource.PlayOneShot(EffectAudio[1], AudioSlider.AudioVoloume);
        objectPool.SpawnFromPool("GroundAttack", new Vector3(Random.Range(-10,10), AreaGroundPoint.position.y, AreaGroundPoint.position.z), AreaGroundPoint.rotation);
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
        objectPool.SpawnFromPool("Tesla", TeslaPoint[0].position, transform.rotation);
        audioSource.PlayOneShot(EffectAudio[3], AudioSlider.AudioVoloume);
        IdleState();
    }

    void FinalAttackEvent()
    {
        Time.timeScale = 0.5f;
        audioSource.PlayOneShot(EffectAudio[4], AudioSlider.AudioVoloume);
        anim.SetTrigger("FinalHand");
        PlayerTarget.GetComponent<PlayerLV5>().isCanMove = false;
        Invoke("FinalAttackEvent2", 1f);
        FinalAttack.SetActive(true);
    }

    void FinalAttackEvent2()
    {
        Instantiate(MomEffect, MomEffectPoint.position, MomEffect.transform.rotation);
        RunnerKingState = State.MOMAPPEAR;
    }

    IEnumerator ChangeButton()
    {
        yield return new WaitForSeconds(5f);
        ProtectPlane.enabled = true;
        yield return new WaitForSeconds(1f);
        isChangeButton = true;
    }

    IEnumerator ChangeButton2()
    {
        yield return new WaitForSeconds(1f);
        UseObjButton.gameObject.SetActive(false);
        AppearTime = 0;
        player.enabled = true;
        ProtectText.enabled = true;
        Time.timeScale = 0;
        NextButton.SetActive(true);
    }

    public void ProtectTextContinue()
    {
        ProtectPlane.enabled = false;
        ProtectText.enabled = false;
        Time.timeScale = 1;
    }
}
