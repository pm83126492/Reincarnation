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
    private AudioSource audioSource;

    public GameObject GhostCamera;
    public GameObject DrawObject, DrawCanvas;

    bool isFlashRed;
    bool isEnemyDie;
    bool isDrawUI;
    bool isPlayAudio;

    public bool GhostIsOut;


    float RedTime, BlackTime, SignAppearTime;
    float EnemyToPlayerDistance;

    private Vignette vignette;
    public Volume volume;
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
            Destroy(player.hit2.collider.gameObject);
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
                if (LineCollider.ColliderNumber == 6&&!isDrawUI)
                {
                    isDrawUI = true;
                    DrawCanvas.SetActive(false);
                    DrawObject.SetActive(false);
                    GhostState = State.SPELL;
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
                GhostAI.enabled = false;
                player.anim.SetBool("Spells", true);
                StartCoroutine(SpellEffectOpen());
                GhostState = State.SPELLTOPASS;
                break;

            case State.PASS:
                FlashRedLight();
                GhostAI.target = GhostAI.target2;
                GhostAI.enabled = true;
                player.anim.SetBool("Spells", false);
                player.PlayerRenderer.material.shader = OutlineShader;
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
