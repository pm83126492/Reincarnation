using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameControllerLv1 : MonoBehaviour
{
    public Animator BlackAnim;
    public BlackFade blackFade;
    public MirrorTouch mirrorTouch;
    public CircleHeartBeatTest circleHeartBeat;
    public CanvasGroup MirrorCanvasGroup,BGMirrorCanvasGroup;
    public Player player;
    public Transform MidPoint;
    public BoxCollider2D WallCollider;
    float CanvasGroupTimer;
    bool isWin;
    bool isFail;

    public UniversalRenderPipelineAsset cameraData;
    public RenderPipelineAsset renderPipeline;
    public CinemachineVirtualCamera virtualCamera;

    public AudioSource audioSource;
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
        MirrorCanvasGroup.gameObject.SetActive(false);
        BGMirrorCanvasGroup.gameObject.SetActive(false);
        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        audioSource = GetComponent<AudioSource>();
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.85f;
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
                    mirrorTouch.playableDirector.Play();
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

        CanGoLV2();

        ReloadLV1();
    }

    void ReloadLV1()
    {
        if (isFail)
        {
            CanvasGroupTimer += Time.deltaTime;
            MirrorCanvasGroup.alpha = BGMirrorCanvasGroup.alpha= 1 - CanvasGroupTimer / 1;
            if (MirrorCanvasGroup.alpha == 0)
            {
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
        Camera.main.orthographic = false;
        isFail = true;
        CanvasGroupTimer = 0;
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
}
