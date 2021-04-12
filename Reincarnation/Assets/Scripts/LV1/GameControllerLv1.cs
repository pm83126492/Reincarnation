using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Cinemachine;

public class GameControllerLv1 : MonoBehaviour
{
    public Material SealMaterial;//封印圖騰材質

    public ParticleSystem ArtefactEmbers;//碎片解謎成功特效

    public Animator BlackAnim;//黑頻動畫
    public Animator CowAnim,HorseAnim;//牛頭馬面動畫
    public Animator GhostAnim;//鬼差動畫

    public BlackFade blackFade;//黑頻程式
    public MirrorTouch mirrorTouch;//點擊Mirror程式
    public CircleHeartBeat circleHeartBeat;//碎片解謎心跳跳動程式
    public Player player;//玩家程式
    public EnemyAI Ghost;//鬼差程式

    public Transform MidPoint;//中點(解謎中回到中點)
    public Transform ReloadPoint;//重新關卡出生點

    public BoxCollider2D ExportEffectCollider;//出口特效Collider

    public Canvas MirrorCanvas, BGMirrorCanvas;//MirrorCanva
    public CanvasGroup MirrorCanvasGroup, BGMirrorCanvasGroup;//MirrorCanvaGroup

    public UniversalRenderPipelineAsset cameraData;//攝影機
    public CinemachineVirtualCamera virtualCamera;//攝影機

    public RenderPipelineAsset renderPipeline;//渲染方式

    public PlayableDirector playableDirector;//TimeLine播放

    public AudioSource audioSource;//音效
    public AudioClip BrokeGlassAudio;//玻璃破碎音效
    public AudioClip ScreamAudio;//鬼叫聲音效
    public AudioClip ChokingAudio;//被掐脖子音效
    public AudioClip DissolveAudio;//Dissolve音效

    bool isParse;//TimeLins暫停中;
    bool isCine;//鏡頭震動中
    public bool isWin;//關卡過關
    bool isFail;//關卡失敗
    bool isReloadScence;//重新開始中
    bool isDieEffect;//死亡效果中

    float CineTimer;//鏡頭震動時間
    float DissolveTimer;//封印圖騰材質Dissolve時間  
    float CanvasGroupTimer;//CanvasGroup時間
    float PlayerIsDieTime;//Player準備被纏脖子時間
    public float Radius;//鏡子破裂爆炸範圍
    public float Force;//鏡子破裂爆炸力道
    float ColliderOpenTime;//鏡子碰觸Collider打開倒數時間
    float FailTimer;//失敗時間計時時間
    float HzTime;//心跳頻率計時時間
    int HzNumber;//心跳頻率等級

    public GameObject EyesLight;//點擊Mirror氣氛轉換物件
    public GameObject MirrorCrack;//Mirror碎片物件
    public GameObject MirrorComplete;//Mirror完整物件

    public enum state
    {
        NONE,
        MIRROR,
        PUZZLE,
        END,
    }
    public state GameState;//鏡子狀態

    void Start()
    {
        Drag2.MirrorCrackNumber = 0;
        SceneSingleton._Instance.SetState(0);
        MirrorCanvas.enabled = false;
        BGMirrorCanvas.enabled = false;

        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.85f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 2f;

        audioSource = GetComponent<AudioSource>();

        SealMaterial.SetFloat("_DissolveAmount", 0);

        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            player.transform.position = ReloadPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (GameState)
        {
            case state.MIRROR://鏡子進入解謎狀態
                EyesLight.SetActive(false);
                CanvasGroupTimer += Time.deltaTime;
                MirrorCanvas.enabled = true;
                BGMirrorCanvas.enabled = true;
                MirrorCanvasGroup.alpha = BGMirrorCanvasGroup .alpha= CanvasGroupTimer / 1;
                if (MirrorCanvasGroup.alpha >= 1)
                {
                    Camera.main.orthographic = true;
                    player.transform.position = MidPoint.position;
                    audioSource.Play();
                    circleHeartBeat.gameObject.SetActive(true);
                   // StartCoroutine(FailTime());
                   // StartCoroutine(HeartBreatHz());
                    GameState = state.PUZZLE;
                }
                break;

            case state.PUZZLE://鏡子解謎中狀態
                FailTimer += Time.deltaTime;
                HzTime += Time.deltaTime;

                if (HzTime >= 12&& HzNumber==0)
                {
                    circleHeartBeat.rangeSpeed = 530;
                    HzNumber += 1;
                    HzTime = 0;
                }else if(HzTime >= 13 && HzNumber == 1)
                {
                    circleHeartBeat.rangeSpeed = 680;
                    HzNumber += 1;
                    HzTime = 0;
                }
                else if (HzTime >= 8 && HzNumber == 2)
                {
                    circleHeartBeat.rangeSpeed = 790;
                    HzNumber += 1;
                    HzTime = 0;
                }

                if (Drag2.MirrorCrackNumber == 13)
                {
                    isWin = true;
                    playableDirector.Play();
                    ArtefactEmbers.Play();
                    Camera.main.orthographic = false;
                    circleHeartBeat.gameObject.SetActive(false);
                    ExportEffectCollider.gameObject.SetActive(true);
                    audioSource.Stop();
                    GameState = state.END;
                }

                if (FailTimer >= 40f)
                {
                    Camera.main.orthographic = false;
                    EyesLight.SetActive(true);
                    isFail = true;
                    audioSource.Stop();
                    circleHeartBeat.gameObject.SetActive(false);
                    Ghost.gameObject.SetActive(true);
                    CanvasGroupTimer = 0;
                    GameState = state.NONE;
                }
                break;

            case state.END://鏡子解謎結束狀態
                ColliderOpenTime += Time.deltaTime;
                if (ColliderOpenTime >= 7)
                {
                    MirrorCanvas.enabled = false;
                    BGMirrorCanvas.enabled = false;
                    mirrorTouch.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    player.isCanMove = true;
                    GameState = state.NONE;
                }
                break;
        }

        OpenMirrorCanvas();

        CanGoLV2();

        ReloadLV1();
    }

    public void TouchMirror()//點擊鏡子事件
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3;
        EyesLight.SetActive(true);
        player.isCanMove = false;
        isCine = true;
        AudioManager.Instance.PlaySource("MirrorScream", 1,"1");
        //audioSource.PlayOneShot(ScreamAudio);
        mirrorTouch.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Play());
    }

    void OpenMirrorCanvas()//點擊鏡子後動畫事件
    {
        if (float.Parse(playableDirector.time.ToString("0.0")) > 0f)
        {
            if (DissolveTimer == 0)
            {
                AudioManager.Instance.PlaySource("Dissolve", 1,"0");
                //audioSource.PlayOneShot(DissolveAudio);
            }
            DissolveTimer += Time.deltaTime;
            SealMaterial.SetFloat("_DissolveAmount", Mathf.Clamp(DissolveTimer / 1, 0, 1.1f));
        }

        if (float.Parse(playableDirector.time.ToString("0.0")) == 4f && !isParse)
        {
            MirrorComplete.transform.position = new Vector3(100, 0, 0);
            MirrorCrack.gameObject.SetActive(true);
            ArtefactEmbers.Pause();
            AudioManager.Instance.PlaySource("BrokeGlass", 0.3f,"1");
            //audioSource.PlayOneShot(BrokeGlassAudio);
            CowAnim.SetBool("isMirror", false);
            HorseAnim.SetBool("isMirror", false);
            playableDirector.Pause();
            MirrorCanvasGroup.alpha = 0;
            Explose();
            StartCoroutine(EffectPlay());
            isParse = true;
        }

        if (isCine)
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, 0, Time.deltaTime * 3);
        }
    }

    void ReloadLV1()//重新開始
    {
        if (isFail)
        {
            float EnemyToPlayerDistance = Vector2.Distance(Ghost.transform.position, player.transform.position);
            //Debug.Log(EnemyToPlayerDistance);
            CanvasGroupTimer += Time.deltaTime;
            MirrorCanvasGroup.alpha = BGMirrorCanvasGroup.alpha= 1 - CanvasGroupTimer / 1;
            if (MirrorCanvasGroup.alpha == 0&& EnemyToPlayerDistance<=4.1f&&!isReloadScence)
            {
                PlayerIsDieTime += Time.deltaTime;
                Ghost.enabled = false;
                GhostAnim.SetTrigger("TongueAttack");
                if (PlayerIsDieTime >= 1f)
                {
                    if (!isDieEffect)
                    {
                        //audioSource.PlayOneShot(ChokingAudio);
                        AudioManager.Instance.PlaySource("PlayerScream", 1,"other");
                        player.anim.SetBool("GhostWhiteAttack",true);
                        isDieEffect = true;
                    }
                    if (PlayerIsDieTime >= 3f)
                    {
                        SceneSingleton._Instance.SetState(2);
                    }
                }
            }
        }

        /*if (isReloadScence)
        {
             BlackAnim.SetTrigger("FadeOut");
             if (blackFade.CanChangeScene)
             {
                 SceneManager.LoadScene("LV1");
             }
        }*/
    }

    void CanGoLV2()//前往下一關
    {
        if (player.isObstacle == true && player.hit2.collider.gameObject.tag == "Export")
        {
            SceneSingleton._Instance.SetState(1);
            player.isCanMove = false;
            //BlackAnim.SetTrigger("FadeOut");
        }

       /* if (blackFade.CanChangeScene)// && isWin)
        {
            SceneManager.LoadScene("LV2");
        }*/
    }

    void Explose()//鏡子爆破事件
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(mirrorTouch.gameObject.transform.position, Radius);
        foreach(Collider2D nearbyObject in collider2D)
        {
            nearbyObject.GetComponent<Rigidbody2D>().isKinematic = false;
            Vector2 direction = nearbyObject.transform.position - mirrorTouch.gameObject.transform.position;
            nearbyObject.GetComponent<Rigidbody2D>().AddForce(direction*Force);
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mirrorTouch.gameObject.transform.position, Radius);
    }*/

   /* IEnumerator FailTime()//失敗時間計時
    {
        yield return new WaitForSeconds(40f);
        if (GameState == state.PUZZLE)
        {
            circleHeartBeat.gameObject.SetActive(false);
            Camera.main.orthographic = false;
            isFail = true;
            audioSource.Stop();
            Ghost.gameObject.SetActive(true);
            CanvasGroupTimer = 0;
        }
    }*/

   /* IEnumerator HeartBreatHz()//心跳跳動頻率跟隨心跳音效
    {
        yield return new WaitForSeconds(12f);
        circleHeartBeat.rangeSpeed = 530;
        yield return new WaitForSeconds(13f);
        circleHeartBeat.rangeSpeed = 680;
        yield return new WaitForSeconds(8);
        circleHeartBeat.rangeSpeed = 790;
    }*/

    IEnumerator Play()//PlayTimeLine
    {
        yield return new WaitForSeconds(3f);
        CowAnim.SetBool("isMirror", true);
        HorseAnim.SetBool("isMirror", true);
        playableDirector.Play();
    }

    IEnumerator EffectPlay()//進入鏡子解謎倒數
    {
        yield return new WaitForSeconds(3f);

        MirrorCanvasGroup.gameObject.SetActive(true);
        GameState = state.MIRROR;
    }
}
