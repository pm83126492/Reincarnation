using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Cinemachine;

[RequireComponent(typeof(Explodable))]
public class GameControllerLv1 : MonoBehaviour
{
    public Material SealMaterial;
    public ParticleSystem ArtefactEmbers;
    public Animator BlackAnim;
    public Animator CowAnim,HorseAnim;
    public BlackFade blackFade;
    public MirrorTouch mirrorTouch;
    public CircleHeartBeat circleHeartBeat;
    public CanvasGroup MirrorCanvasGroup,BGMirrorCanvasGroup;
    public Player player;
    public EnemyAI Ghost;
    public Transform MidPoint;
    public BoxCollider2D WallCollider;
    float CanvasGroupTimer;
    bool isWin;
    bool isFail;

    public UniversalRenderPipelineAsset cameraData;
    public RenderPipelineAsset renderPipeline;
    public CinemachineVirtualCamera virtualCamera;

    public AudioSource audioSource;

    public ExplodeOnClick _explodable;
    public PlayableDirector playableDirector;
    public AudioClip BrokeGlassAudio;
    public AudioClip ScreamAudio;
    bool isParse;
    bool isCine;
    float CineTimer;
    float DissolveTimer;
    public GameObject Spell;
    public GameObject EyesLight;

    public enum state
    {
        NONE,
        MIRROR,
        PUZZLE,
        END,
    }
    public state GameState;
    // Start is called before the first frame update
    void Start()
    {
        Drag2.MirrorCrackNumber = 0;
        MirrorCanvasGroup.gameObject.SetActive(false);
        BGMirrorCanvasGroup.gameObject.SetActive(false);
        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        audioSource = GetComponent<AudioSource>();
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.85f;
        SealMaterial.SetFloat("_DissolveAmount", 0);
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (GameState)
        {
            case state.MIRROR:
                player.enabled = false;
                CanvasGroupTimer += Time.deltaTime;
                MirrorCanvasGroup.gameObject.SetActive(true);
                BGMirrorCanvasGroup.gameObject.SetActive(true);
                MirrorCanvasGroup.alpha = BGMirrorCanvasGroup .alpha= CanvasGroupTimer / 1;
                if (MirrorCanvasGroup.alpha >= 1)
                {
                    Camera.main.orthographic = true;
                    player.transform.position = MidPoint.position;
                    audioSource.Play();
                    circleHeartBeat.gameObject.SetActive(true);
                    StartCoroutine(FailTime());
                    StartCoroutine(HeartBreatHz());
                    GameState = state.PUZZLE;
                }
                break;

            case state.PUZZLE:
                if (Drag2.MirrorCrackNumber == 13)
                {
                    isWin = true;
                    playableDirector.Play();
                    ArtefactEmbers.Play();
                    Camera.main.orthographic = false;
                    circleHeartBeat.gameObject.SetActive(false);
                    audioSource.Stop();
                    GameState = state.END;
                }
                break;

            case state.END:
                player.enabled = true;
                break;
        }

        OpenMirrorCanvas();

        CanGoLV2();

        ReloadLV1();
    }

    public void TouchMirror()
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3;
        EyesLight.SetActive(true);
        isCine = true;
        audioSource.PlayOneShot(ScreamAudio);
        mirrorTouch.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Play());
    }

    void OpenMirrorCanvas()
    {
        if (float.Parse(playableDirector.time.ToString("0.0")) > 0f)
        {
            DissolveTimer += Time.deltaTime;
            SealMaterial.SetFloat("_DissolveAmount", Mathf.Clamp(DissolveTimer / 2, 0, 1.1f));
        }

        if (float.Parse(playableDirector.time.ToString("0.0")) == 4f && !isParse)
        {
            //Spell.SetActive(false);
            ArtefactEmbers.Pause();
            audioSource.PlayOneShot(BrokeGlassAudio);
            CowAnim.SetBool("isMirror", false);
            HorseAnim.SetBool("isMirror", false);
            playableDirector.Pause();
            MirrorCanvasGroup.alpha = 0;
            _explodable.Explosion();
            StartCoroutine(EffectPlay());
            isParse = true;
        }

        if (isCine)
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, 0, Time.deltaTime * 3);
        }
    }

    void ReloadLV1()
    {
        if (isFail)
        {
            float EnemyToPlayerDistance = Vector2.Distance(Ghost.transform.position, player.transform.position);
            Debug.Log(EnemyToPlayerDistance);
            CanvasGroupTimer += Time.deltaTime;
            MirrorCanvasGroup.alpha = BGMirrorCanvasGroup.alpha= 1 - CanvasGroupTimer / 1;
            if (MirrorCanvasGroup.alpha == 0&& EnemyToPlayerDistance<=3)
            {
                Ghost.enabled = false;
                BlackAnim.SetTrigger("FadeOut");
                if (blackFade.CanChangeScene)
                {
                    SceneManager.LoadScene("LV1");
                }
            }
        }
    }

    void CanGoLV2()
    {
        if (isWin)
        {
            WallCollider.enabled = false;
        }

        if (player.transform.position.x >= 40)
        {
            isWin = true;
            BlackAnim.SetTrigger("FadeOut");
        }

        if (blackFade.CanChangeScene && isWin)
        {
            SceneManager.LoadScene("LV2");
        }
    }

    IEnumerator FailTime()
    {
        yield return new WaitForSeconds(40f);
        if (GameState == state.PUZZLE)
        {
            circleHeartBeat.gameObject.SetActive(false);
           // mirrorTouch.EyesLight.SetActive(false);
            Camera.main.orthographic = false;
            isFail = true;
            Ghost.gameObject.SetActive(true);
            CanvasGroupTimer = 0;
        }
    }

    IEnumerator HeartBreatHz()
    {
        yield return new WaitForSeconds(12f);
        circleHeartBeat.rangeSpeed = 530;
        yield return new WaitForSeconds(13f);
        circleHeartBeat.rangeSpeed = 680;
        yield return new WaitForSeconds(8);
        circleHeartBeat.rangeSpeed = 790;
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(3f);
        CowAnim.SetBool("isMirror", true);
        HorseAnim.SetBool("isMirror", true);
        playableDirector.Play();
    }

    IEnumerator EffectPlay()
    {
        yield return new WaitForSeconds(3f);

        MirrorCanvasGroup.gameObject.SetActive(true);
        GameState = state.MIRROR;
    }
}
