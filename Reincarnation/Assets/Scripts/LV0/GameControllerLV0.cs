using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;

public class GameControllerLV0 : MonoBehaviour
{
    public Canvas DoorCanvas;
    public Canvas TeachUI;
    public Canvas SkipUI;
    public Canvas GetStickUI;

    public CanvasGroup DoorCanvasGroup;
    public CanvasGroup DoorCicleFlowerCanvasGroup;

    public bool IsWin;
    public bool IsWin02;
    bool isUseObjUI;

    public Player player;
    public BlackFade blackFade;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera virtualCamera;

    public Material DoorFlowerLight;
    public Material DoorCircleLightMaterial;
    public Material DoorCrackHDR;
    public Material DoorFlowerDissolveMaterial;

    private float ColorAmount;
    private float LightTimer;
    private float DissolveTimer;
    private float CanvasGroupTimer;
    private float CanvasGroupDissloveTimer;
    private float CrackHDRTimer;
    private float OneDuration = 1f;
    private float TwoDuration = 2f;
    private float CanNotMoveTime;
    float SlideTime;

    public Animation anim;
    public Animation DoorUIanim;
    public Animation Dooranim;
    public Animator PlayerAnim;
    public Animator BlockFadeAnim;
    public RuntimeAnimatorController trickAnim,NotrickAnim;

    public GameObject bloom;
    public GameObject Door;
    public GameObject DoorFlower;
    public GameObject DoorOpen;
    public GameObject EnemyUI;
    public GameObject DrawUI;
    public GameObject GhostAttackUI;
    public GameObject TeachObject;
    public GameObject Door_CorkRed_Under;
    public GameObject[] Door_LR;
    public GameObject[] Doorlock_LR;

    public BoxCollider2D DoorCollider, UIInvisibleWall;
    public BoxCollider2D DoorWinCollider;
    public GameObject Ghost;

    public GhostControllder ghostControllder;
    public enum state
    {
        NONE,
        RightMove,
        LeftMove,
        JumpMove,
        SlideMove,
        PleaseObj,
        UseObj,
        EnemyAppearUI,
        DrawAppearUI,
        FinishDrawUI,
        StartGame,
        DoorLightFadeIn,
        DoorCanAnim,
        DoorDissolve,
        DoorWin,
        DoorOver,
        STOP,
        STOP2,
    }
    public state GameState;
    // Start is called before the first frame update
    void Start()
    {
        //player.isCanMove = false;
        PlayerAnim.runtimeAnimatorController = NotrickAnim as RuntimeAnimatorController;
        bloom.SetActive(false);
        Door.SetActive(true);
        DoorFlower.SetActive(true);
        DoorOpen.SetActive(false);
        StartCoroutine(StartGame());
        GameState = state.NONE;
        IsWin = IsWin02 = isUseObjUI= false;
        DoorCanvasGroup.alpha = 0;
        DoorCanvas.enabled=false;
        //TeachUI.enabled = false;
        DoorWinCollider.enabled = false;
        DoorFlowerLight.SetFloat("_Amount", 0);
        DoorCircleLightMaterial.SetFloat("_OutlineThickness", 0);
        DoorFlowerDissolveMaterial.SetFloat("_DissolveAmount", 0);
        DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(0, 0, 0, 0));
        DoorCrackHDR.SetFloat("_ColorAmount", 0);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.72f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayableTime();
        //關卡門狀態機
        DoorState();

        //解謎成功
        if (IsWin == true && IsWin02 == true)
        {
            GameState = state.DoorWin;
        }

        if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV1");
        }

       /* if (ghostControllder.isGhostAttackDie)//|| player.CanChangeScene)
        {
            BlockFadeAnim.SetTrigger("FadeOut");
        }


        if (blackFade.CanChangeScene && !IsWin)
        {
            SceneManager.LoadScene("LV0");
        }*/

        UIEvent();
        
    }

    public void DoorCanOpen()
    {
        if (player.isObstacle == true)
        {
            DoorCanvas.enabled = true;
            GameState = state.DoorLightFadeIn;
        }
    }

    public void TouchFlowerCircle()
    {
        if (DoorCanvasGroup.alpha >= 1&& DoorCircleLightMaterial.GetFloat("_OutlineThickness")<=0)
        {
            anim.Play();
            GameState = state.DoorCanAnim;
            StartCoroutine(BoolDoorFlowerDissolve());
            DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(255, 100, 0, 255) * 0.004f);
            LightTimer = 0;
        }
    }

    IEnumerator BoolDoorFlowerDissolve()
    {
        yield return new WaitForSeconds(3);
        GameState = state.DoorDissolve;
    }

    void DoorState()
    {
        switch (GameState)
        {
            case state.EnemyAppearUI:

                if (player.isObstacle == true && player.hit2.collider.gameObject.tag == "EnemyAppearCollider")
                {
                    ghostControllder.enabled = true;
                    StartCoroutine(PauseGame());
                    GameState = state.DrawAppearUI;
                }

                break;
                
            case state.DrawAppearUI:
                if (ghostControllder.SignCanvasGroup.alpha >= 0.98f)
                {
                    DrawUI.SetActive(true);
                    GameState = state.FinishDrawUI;
                    Time.timeScale = 0;
                    ghostControllder.audioSource.Pause();
                }
                break;

            case state.FinishDrawUI:
                if (ghostControllder.GhostIsOut)
                {
                    player.isCanMove = false;
                    StartCoroutine(OpenGetStickUI());
                    GameState = state.NONE;
                }

                if (ghostControllder.isGhostAttackDie)
                {
                    StartCoroutine(GhostAttack());
                    GameState = state.NONE;
                }
                break;
                
            case state.NONE:
                break;

            case state.STOP:
                player.anim.SetFloat("WalkSpeed", 0);
                player.anim.SetBool("Slide", false);
                player.anim.SetBool("SquatPush", false);
                player.obstacle = null;
                player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
                player.isCanMove = false;
                player.isSlide = false;
                break;

            //RightMove
            case state.RightMove:
                player.isCanMove = true;
                if(player.hit2.collider!=null&&player.hit2.collider.gameObject.tag == "arrow")
                {
                    playableDirector.Play();
                    GameState = state.STOP;
                }
                break;

            //LeftMove
            case state.LeftMove:
                player.isCanMove = true;
                if (player.hit2.collider != null && player.hit2.collider.gameObject.tag == "arrowLeft")
                {
                    playableDirector.Play();
                    GameState = state.STOP;
                }
                break;

            //JumpMove
            case state.JumpMove:
                player.isCanMove = true;
                if (player.transform.position.y > 0)
                {
                    playableDirector.Play();
                    GameState = state.STOP;
                }
                break;

            //SlideMove
            case state.SlideMove:
                player.isCanMove = true;
                if (player.isSlide ==true)
                {
                    SlideTime += Time.deltaTime;
                    if (SlideTime > 0.7f)
                    {
                        GameState = state.STOP;
                        playableDirector.Play();
                    }
                }
                break;

            //PleaseObj
            case state.PleaseObj:
                player.isCanMove = true;
                if (player.hit2.collider != null && player.hit2.collider.gameObject.tag == "smallobstacle")
                {
                    playableDirector.Play();
                    GameState = state.STOP;
                }
                break;

            //UseObj
            case state.UseObj:
                player.isCanMove = true;
                if (player.obstacle!=null&&player.obstacle.transform.position.x>-5f)
                {
                    playableDirector.Play();
                    Ghost.SetActive(true);
                    GameState = state.EnemyAppearUI;
                }
                break;

            //小圖門花紋發光
            case state.DoorLightFadeIn:
                PlayerAnim.SetBool("Staff", true);
                LightTimer += Time.deltaTime;
                if (LightTimer >= 4)
                {
                    CanvasGroupTimer += Time.deltaTime;
                    DoorCanvasGroup.alpha = CanvasGroupTimer / OneDuration;
                }
                break;

            //解謎門動畫    
            case state.DoorCanAnim:
                bloom.SetActive(true);
                LightTimer += Time.deltaTime;
                DoorCircleLightMaterial.SetFloat("_OutlineThickness", Mathf.Clamp(LightTimer / OneDuration, 0, 2));
                break;

            //花紋消失
            case state.DoorDissolve:
                DissolveTimer += Time.deltaTime;
                DoorFlowerDissolveMaterial.SetFloat("_DissolveAmount", Mathf.Clamp(DissolveTimer / OneDuration, 0, 1.1f));
                DoorCicleFlowerCanvasGroup.alpha = 1 - DissolveTimer / OneDuration;
                Camera.main.orthographic = true;
                player.enabled = false;
                break;

            //解謎成功
            case state.DoorWin:
                PlayerAnim.SetBool("Staff", false);
                DoorCrackHDR.SetFloat("_ColorAmount", 1.5f);
                DoorUIanim.Play();
                StartCoroutine(DoorOpenAnim());
                StartCoroutine(EnableDoor());
                ColorAmount = DoorCrackHDR.GetFloat("_ColorAmount");
                Door_CorkRed_Under.transform.parent = Door_LR[0].transform;
                Doorlock_LR[0].transform.parent = Door_LR[0].transform;
                Doorlock_LR[1].transform.parent = Door_LR[1].transform;
                break;

            //關掉解謎門
            case state.DoorOver:
                Camera.main.orthographic = false;
                CrackHDRTimer += Time.deltaTime;
                CanvasGroupDissloveTimer += Time.deltaTime;
                DoorCanvasGroup.alpha = 1 - CanvasGroupDissloveTimer / TwoDuration;
                DoorFlowerLight.SetFloat("_Amount", 0);
                IsWin = IsWin02 = false;
                Door.SetActive(false);
                DoorFlower.SetActive(false);
                DoorOpen.SetActive(true);
                player.enabled = true;
                DoorWinCollider.enabled = true;
                if (DoorCanvasGroup.alpha <= 0)
                {
                    bloom.SetActive(false);
                    GameState = state.NONE;
                }
                break;
        }
    }
    IEnumerator DoorOpenAnim()
    {
        yield return new WaitForSeconds(2f);
        Dooranim.Play();
    }

    IEnumerator EnableDoor()
    {
        yield return new WaitForSeconds(3f);
        DoorCollider.enabled=false;
        CrackHDRTimer = 0;
        GameState = state.DoorOver;
    }

   /* void MobileTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //第一隻手指移動中
            if (touch.phase == TouchPhase.Began)
            {
                if (GameState == state.RightMove)
                {
                    TeachUI.enabled = false;
                    RightMoveUI.SetActive(false);
                    GameState = state.STOP;
                }
                else if (GameState == state.LeftMove)
                {
                    TeachUI.enabled = false;
                    LeftMoveUI.SetActive(false);
                    GameState = state.STOP;
                }
                else if (GameState == state.JumpMove)
                {
                    TeachUI.enabled = false;
                    JumpMoveUI.SetActive(false);
                    GameState = state.STOP;
                }
                else if (GameState == state.SlideMove)
                {
                    TeachUI.enabled = false;
                    SlideMoveUI.SetActive(false);
                    UIInvisibleWall.enabled = false;
                    GameState = state.NONE;
                }
                else if (GameState == state.UseObj)
                {
                    GameState = state.NONE;
                    TeachUI.enabled = false;
                    UseObjUI.SetActive(false);
                }
                CanNotMoveTime = 0;
            }
        }
    }*/

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        SkipUI.enabled = true;
        Time.timeScale = 0;
    }

    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(1f);
        ghostControllder.GhostAI.enabled = true;
        EnemyUI.SetActive(true);
        Time.timeScale = 0;
        ghostControllder.audioSource.Pause();
    }

    void PlayableTime()
    {
        if (float.Parse(playableDirector.time.ToString("0.0"))==5.5f)
        {
            playableDirector.Pause();
            GameState = state.RightMove;
        }
        else if(float.Parse(playableDirector.time.ToString("0.0")) == 11f)
        {
            playableDirector.Pause();
            GameState = state.LeftMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 16f)
        {
            playableDirector.Pause();
            GameState = state.JumpMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 24f)
        {
            playableDirector.Pause();
            GameState = state.SlideMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 25f)
        {
            playableDirector.Pause();
            GameState = state.PleaseObj;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 30f)
        {
            playableDirector.Pause();
            GameState = state.UseObj;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BlockFadeAnim.SetTrigger("FadeOut");
        }
    }

    void UIEvent()
    {
        if (player.isObstacle == true && !isUseObjUI&& player.hit2.collider.gameObject.tag == "smallobstacle")
        {
            GameState = state.UseObj;
            isUseObjUI = true;
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        TeachUI.enabled = false;
    }

    public void SkipButton()
    {
        //Time.timeScale = 1;
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        SkipUI.enabled = false;
        GetStickUI.enabled = true;
        //player.isCanMove = true;
    }

    public void CloseGetStickUI()
    {
        Time.timeScale = 1;
        GetStickUI.enabled = false;
        player.isCanMove = true;
    }

    public void CloseNoStickUI()
    {
        Time.timeScale = 1;       
    }

    public void NoSkipButton()
    {
        Time.timeScale = 1;
        TeachUI.enabled = true;
        playableDirector.Play();
        GameState = state.STOP;
        SkipUI.enabled = false;
    }

    public void EnemyUIContinue()
    {
        EnemyUI.SetActive(false);
        ghostControllder.audioSource.Play();
        Time.timeScale = 1;
    }

    public void DrawUIContinue()
    {
        DrawUI.SetActive(false);
        ghostControllder.audioSource.Play();
        Time.timeScale = 1;
    }

    public void GhostAttackUIContinue()
    {
        ghostControllder.vignette.color.value = new Color(0f, 0f, 0f);
        GhostAttackUI.SetActive(false);
        GetStickUI.enabled = true;
        Ghost.SetActive(false);
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
    }

    IEnumerator GhostAttack()
    {
        yield return new WaitForSeconds(1f);
        GhostAttackUI.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator OpenGetStickUI()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        GetStickUI.enabled = true;
    }
}
