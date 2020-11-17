using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.SceneManagement;

public class DrawEnemy : MonoBehaviour
{
    private Vignette vignette;
    public Volume volume;
    public CinemachineVirtualCamera virtualCamera;
    public Player player;
    public EnemyAI enemyAI;

    public CanvasGroup SignCanvasGroup;

    public GameObject DrawObject, DrawCanvas;
    public GameObject DisappearEffect, EnemyTransform;
    public GameObject WildFireEffect;

    public Animator EnemyAnim;

    public bool isFlashRed;
    public bool isEnemyDie;
    bool isUseDrawUI;

    public float FlashTime;
    float EnemyToPlayerDistance;
    public float RedTime, BlackTime, CineTime,SignAppearTime,TestTime;
    public int FlashingNumber;

    public ParticleSystem FormationEffect;
    // Start is called before the first frame update
    void Start()
    {
        Vignette tmp;
        if (volume.profile.TryGet<Vignette>(out tmp))
        {
            vignette = tmp;
        }
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
        DrawObject.SetActive(false);
        DrawCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (LineCollider.ColliderNumber == 6)
        {
            CineTime += Time.deltaTime;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Mathf.Clamp(CineTime / 5f, 0.2f, 0.5f);
        }*/
        DrawLineEvent();
        EnemyAppear();

        
    }

    void EnemyAppear()
    {
        if (player.transform.position.x >= 3 && !isUseDrawUI)
        {
            if (enemyAI.enabled == false)
            {
                //EnemyComingFlashing(FlashingNumber, FlashTime);
            }
            enemyAI.enabled = true;
        }

        if (EnemyToPlayerDistance <= 25f && !isUseDrawUI)
        {
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
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    vignette.smoothness.value = Mathf.Clamp(RedTime / 0.75f, 0.37f, 1.37f);
                }
                vignette.color.value = new Color(Mathf.Clamp(RedTime / 0.75f, 0, 1f), 0f, 0f);
            }
            else if (isFlashRed)
            {
                BlackTime += Time.deltaTime;
                RedTime = 0;

                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    vignette.smoothness.value = Mathf.Clamp(1f - BlackTime / 0.75f, 0.37f, 1.37f);
                }
                vignette.color.value = new Color(Mathf.Clamp(1f - BlackTime / 0.75f, 0, 1f), 0f, 0f);
            }

            if (EnemyToPlayerDistance <= 20f)
            {
                player.anim.SetFloat("WalkSpeed", 0);
                player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
                player.isCanMove = false;
                CineTime += Time.deltaTime;
                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Mathf.Clamp(0.5f - CineTime / 8f, 0.2f, 0.5f);
            }
        }


    }

    void DrawLineEvent()
    {
        EnemyToPlayerDistance = Vector2.Distance(EnemyTransform.transform.position, player.transform.position);
        //Debug.Log(EnemyToPlayerDistance);
        if(EnemyToPlayerDistance <= 13f&&!isUseDrawUI)
        {
            SignAppearTime += Time.deltaTime;
            DrawCanvas.SetActive(true);
            SignCanvasGroup.alpha = SignAppearTime / 2f;
        }

        if (EnemyToPlayerDistance<=10f&&!isUseDrawUI)
        {
            player.anim.SetFloat("WalkSpeed", 0);
            player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                DrawObject.SetActive(true);
            }
        }

        if (EnemyToPlayerDistance <= 4f && !isUseDrawUI)
        {
            enemyAI.enabled = false;
        }

        if (LineCollider.ColliderNumber == 5)
        {
            isUseDrawUI = true;
            BlackTime = 0f;   
            CineTime = 0f;
            enemyAI.enabled = false;
            DrawCanvas.SetActive(false);
            DrawObject.SetActive(false);
            player.anim.SetBool("Spells", true);
           //FormationEffect.Play();
            StartCoroutine(FormationEffectOpen());
            StartCoroutine(DisappearEffectOpen());
            StartCoroutine(CineBack());
            LineCollider.ColliderNumber += 1;
        }

        if (isEnemyDie)
        {
            CineTime += Time.deltaTime;
            BlackTime+= Time.deltaTime;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Mathf.Clamp(CineTime / 8f, 0.2f, 0.5f);
            vignette.color.value = new Color(Mathf.Clamp(1f - BlackTime / 0.75f, 0, 1f), 0f, 0f);
        }
    }
    IEnumerator FormationEffectOpen()
    {
        yield return new WaitForSeconds(0.5f);
        FormationEffect.Play();
    }

    IEnumerator DisappearEffectOpen()
    {
        yield return new WaitForSeconds(2.5f);
        WildFireEffect.SetActive(false);
        enemyAI.enabled = false;
        EnemyAnim.SetBool("Disappear", true);
        player.anim.SetBool("Spells", false);
        Instantiate(DisappearEffect, EnemyTransform.transform.position, EnemyTransform.transform.rotation);
    }

    IEnumerator CineBack()
    {
        yield return new WaitForSeconds(2f);
        vignette.smoothness.value = 0.37f;
        isEnemyDie = true;
        player.isCanMove = true;
    }
}
