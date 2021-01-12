using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GhostControllder : MonoBehaviour
{
    public Player player;
    public EnemyAI GhostAI;
    public ParticleSystem SpellEffect;
    public Shader OutlineShader;
    public Shader OriginalShader;
    public CanvasGroup SignCanvasGroup;

    public AudioSource audioSource;
    public AudioSource ChokingAudio;//掐脖子音效

    public GameObject GhostCamera;
    public GameObject DrawObject, DrawCanvas;
    public GameObject bloom;

    public Animator GhostAnim;

    bool isFlashRed;
    bool isEnemyDie;
    bool isDrawUI;
    bool isPlayAudio;

    float PlayerIsDieTime;

    public bool GhostIsOut;
    public bool isGhostAttackDie;


    float RedTime, BlackTime, SignAppearTime;
    float EnemyToPlayerDistance;

    public float EnemyToPlayerDistanceMin;

    public Vignette vignette;
    public Volume volume;
    public SortingGroup sortingGroup;
    public enum State
    {
        NONE,
        SPELLTOPASS,
        COMING,
        PASS,
        SPELL,
        FAIL,
        DRAW,
    }

    public State GhostState;
    // Start is called before the first frame update
    void Start()
    {
        GhostAI.enabled = false;
        audioSource = GetComponent<AudioSource>();
        Vignette tmp;
        if (volume.profile.TryGet<Vignette>(out tmp))
        {
            vignette = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GhostAI != null)
        {
            EnemyToPlayerDistance = Vector2.Distance(GhostAI.transform.position, player.transform.position);
        }
        //Debug.Log(EnemyToPlayerDistance);
        if (player.isObstacle == true && player.hit2.collider.gameObject.tag == "EnemyAppearCollider")
        {
            GhostState = State.COMING;
            Destroy(player.hit2.collider.gameObject,1f);
        }
        if (EnemyToPlayerDistance <= 20f && !isDrawUI)
        {
            player.anim.SetFloat("WalkSpeed", 0);
            player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
            player.isCanMove = false;
            GhostCamera.SetActive(true);
            if (EnemyToPlayerDistance <= 15f)
            {
                SignAppearTime += Time.deltaTime;
                DrawCanvas.SetActive(true);
                SignCanvasGroup.alpha = SignAppearTime / 2f;
                if (SignCanvasGroup.alpha == 1)
                {
                    DrawObject.SetActive(true);
                    player.anim.SetBool("Spells", true);
                    GhostState = State.DRAW;
                }
            }
        }

       

        switch (GhostState)
        {
            case State.NONE:

                break;

            case State.SPELLTOPASS:
                FlashRedLight();
                break;

            case State.DRAW:
                FlashRedLight();
                bloom.SetActive(true);
                if (LineCollider.ColliderNumber == 6&&!isDrawUI)
                {
                    isDrawUI = true;
                    DrawCanvas.SetActive(false);
                    DrawObject.SetActive(false);
                    bloom.SetActive(false);
                    player.PlayerRenderer.material.shader = OutlineShader;
                    GhostAI.target = GhostAI.target2;
                    GhostState = State.SPELL;
                }

                if (EnemyToPlayerDistance <= EnemyToPlayerDistanceMin)
                {
                    isDrawUI = true;
                    DrawObject.SetActive(false);
                    bloom.SetActive(false);
                    sortingGroup.sortingOrder = 39;
                    PlayerIsDieTime += Time.deltaTime;
                    GhostAI.enabled = false;
                    GhostAnim.SetTrigger("TongueAttack");
                    if (PlayerIsDieTime >= 1.5f)
                    {
                        ChokingAudio.Play();
                        player.anim.SetTrigger("GhostAttack");
                        player.anim.SetBool("Spells", false);
                        SignAppearTime = 0;
                        GhostState = State.FAIL;
                    }
                }

                break;

            case State.COMING:
                FlashRedLight();
                player.isCanMove = false;
                player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
                GhostAI.enabled = true;
                break;

            case State.SPELL:
                FlashRedLight();
                //GhostAI.enabled = false;
                //player.anim.SetBool("Spells", true);
                StartCoroutine(SpellEffectOpen());
                GhostState = State.SPELLTOPASS;
                break;

            case State.PASS:
                FlashRedLight();
                //GhostAI.target = GhostAI.target2;
                //GhostAI.enabled = true;
                player.anim.SetBool("Spells", false);
                //player.PlayerRenderer.material.shader = OutlineShader;
                if (EnemyToPlayerDistance >= 10 && isDrawUI)
                {
                    player.isCanMove = true;
                    GhostCamera.SetActive(false);
                    Destroy(GhostAI.gameObject, 1f);
                    vignette.color.value = new Color(0f, 0f, 0f);
                    player.PlayerRenderer.material.shader = OriginalShader;
                    if (isPlayAudio)
                    {
                        audioSource.Stop();
                        isPlayAudio = false;
                    }
                    GhostIsOut = true;
                    GhostState = State.NONE;
                }
                break;

            case State.FAIL:
                FlashRedLight();
                GhostCamera.SetActive(false);
                SignAppearTime += Time.deltaTime;
                SignCanvasGroup.alpha = 1-SignAppearTime / 1f;
                if (SignCanvasGroup.alpha <= 0)
                {
                    isGhostAttackDie = true;
                }
                break;
        }
    }

    void FlashRedLight()
    {
        if (!isPlayAudio)
        {
            audioSource.Play();
            isPlayAudio = true;
        }
        if (vignette.color.value.r == 1f)
        {
            isFlashRed = true;
        }
        else if (vignette.color.value.r == 0f)
        {
            isFlashRed = false;
        }

        if (!isFlashRed)
        {
            RedTime += Time.deltaTime;
            BlackTime = 0;
            //vignette.smoothness.value = Mathf.Clamp(RedTime / 0.75f, 0.37f, 0.5f);
            vignette.color.value = new Color(Mathf.Clamp(RedTime / 0.5f, 0, 1f), 0f, 0f);
        }
        else if (isFlashRed)
        {
            BlackTime += Time.deltaTime;
            RedTime = 0;
            // vignette.smoothness.value = Mathf.Clamp(1f - BlackTime / 0.75f, 0.37f, 0.5f);
            vignette.color.value = new Color(Mathf.Clamp(1f - BlackTime / 0.5f, 0, 1f), 0f, 0f);
        }
    }

    IEnumerator SpellEffectOpen()
    {
        yield return new WaitForSeconds(0.5f);
        SpellEffect.Play();
        yield return new WaitForSeconds(2.0f);
        GhostState = State.PASS;
    }

}
