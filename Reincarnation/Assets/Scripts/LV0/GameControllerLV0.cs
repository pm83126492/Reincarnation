﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameControllerLV0 : MonoBehaviour
{
    public Canvas DoorCanvas;
    public Canvas TeachUI;
    public Canvas SkipUI;
    public Canvas GetStickUI;

    public CanvasGroup DoorCanvasGroup;
    public CanvasGroup DoorCicleFlowerCanvasGroup;

    bool Skip;
    public bool IsWin;
    public bool IsWin02;
    public bool isFlashRed;
    bool isUseDrawUI;
    bool isUseObjUI;
    bool isEnemyAppearUI;

    public Player player;
    public BlackFade blackFade;


    public Material DoorFlowerLight;
    public Material DoorCircleLightMaterial;
    public Material DoorCrackHDR;
    public Material DoorFlowerDissolveMaterial;

    private float TeachUIStopTime;
    private float ColorAmount;
    private float LightTimer;
    private float DissolveTimer;
    private float CanvasGroupTimer;
    private float CanvasGroupDissloveTimer;
    private float CrackHDRTimer;
    private float OneDuration = 1f;
    private float TwoDuration = 2f;
    private float CanNotMoveTime;
    float CineTime;

    public Animation anim;
    public Animation Dooranim;
    public Animator PlayerAnim;
    public Animator BlockFadeAnim;
    public RuntimeAnimatorController trickAnim,NotrickAnim;

    public GameObject bloom;
    public GameObject Door;
    public GameObject DoorFlower;
    public GameObject DoorOpen;
    public GameObject RightMoveUI;
    public GameObject LeftMoveUI;
    public GameObject JumpMoveUI;
    public GameObject SlideMoveUI;
    public GameObject UseObjUI;
    public GameObject EnemyUI;
    public GameObject DrawUI;
    public GameObject TeachObject;

    public BoxCollider2D DoorCollider, UIInvisibleWall;
    public BoxCollider2D DoorWinCollider;

    public DrawEnemy drawEnemy;
    public EnemyAI enemyAI;
    public enum state
    {
        NONE,
        STOP,
        RightMove,
        LeftMove,
        JumpMove,
        SlideMove,
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
    }
    public state GameState;
    // Start is called before the first frame update
    void Start()
    {
        PlayerAnim.runtimeAnimatorController = NotrickAnim as RuntimeAnimatorController;
        bloom.SetActive(false);
        enemyAI.enabled = false;
        player.enabled = false;
        Door.SetActive(true);
        DoorFlower.SetActive(true);
        DoorOpen.SetActive(false);
        StartCoroutine(StartGame());
        GameState = state.NONE;
        IsWin = IsWin02 = isUseObjUI = isEnemyAppearUI = false;
        DoorCanvasGroup.alpha = 0;
        DoorCanvas.enabled=false;
        TeachUI.enabled = false;
        DoorWinCollider.enabled = false;
        DoorFlowerLight.SetFloat("_Amount", 0);
        DoorCircleLightMaterial.SetFloat("_OutlineThickness", 0);
        DoorFlowerDissolveMaterial.SetFloat("_DissolveAmount", 0);
        DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(0, 0, 0, 0));
        DoorCrackHDR.SetFloat("_ColorAmount", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //關卡門狀態機
        DoorState();

        //解謎成功
        if (IsWin == true && IsWin02 == true)
        {
            GameState = state.DoorWin;
        }

        if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV2");
        }

        UIEvent();

        if (drawEnemy.isEnemyDie&&drawEnemy.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX==0.5f)
        {
            PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        }
        
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
            DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(255, 100, 0, 255) * 0.005f);
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
            case state.STOP:
                TeachUIStopTime += Time.deltaTime;
                if (TeachUIStopTime >= 2f)
                {
                    player.anim.SetFloat("WalkSpeed", 0);
                    player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
                    player.enabled = false;
                }
                break;

            //RightMove
            case state.RightMove:
                
                TeachUI.enabled = true;
                RightMoveUI.SetActive(true);
                CanNotMoveTime += Time.deltaTime;
                if (CanNotMoveTime >= 1)
                {
                    player.enabled = true;
                    MobileTouch();
                }
                break;

            //LeftMove
            case state.LeftMove:
                TeachUIStopTime = 0;
                player.enabled = true;
                TeachUI.enabled = true;
                LeftMoveUI.SetActive(true);
                CanNotMoveTime += Time.deltaTime;
                if (CanNotMoveTime >= 1)
                {
                    MobileTouch();
                }
                break;

            //JumpMove
            case state.JumpMove:
                TeachUIStopTime = 0;
                player.enabled = true;
                TeachUI.enabled = true;
                JumpMoveUI.SetActive(true);
                CanNotMoveTime += Time.deltaTime;
                if (CanNotMoveTime >= 1)
                {
                    MobileTouch();
                }
                break;

            //SlideMove
            case state.SlideMove:
                TeachUIStopTime = 0;
                player.enabled = true;
                TeachUI.enabled = true;
                SlideMoveUI.SetActive(true);
                CanNotMoveTime += Time.deltaTime;
                if (CanNotMoveTime >= 1)
                {
                    MobileTouch();
                }
                break;

            //UseObj
            case state.UseObj:
                //player.enabled = true;
                TeachUI.enabled = true;
                UseObjUI.SetActive(true);
                CanNotMoveTime += Time.deltaTime;
                if (CanNotMoveTime >= 1)
                {
                    MobileTouch();
                }
                break;

            case state.EnemyAppearUI:
                StartCoroutine(PauseGame());
                player.isCanMove = false;
                player.OneTouchX = player.OneTouchX = player.OneTouchX2 = player.TwoTouchX = player.TwoTouchX2 = player.TwoTouchY = player.TwoTouchY2 = 0;
                GameState = state.DrawAppearUI;
                break;

            case state.DrawAppearUI:
                if (drawEnemy.SignCanvasGroup.alpha == 1)
                {
                    TeachUI.enabled = true;
                    DrawUI.SetActive(true);
                    GameState = state.FinishDrawUI;
                    Time.timeScale = 0;
                }
                break;

            case state.FinishDrawUI:
                if (drawEnemy.isEnemyDie&&drawEnemy.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX==0.5f)
                {
                    Time.timeScale = 0;
                    GetStickUI.enabled = true;
                    GameState = state.NONE;
                }
                break;



            //小圖門花紋發光
            case state.DoorLightFadeIn:
                PlayerAnim.SetBool("Staff", true);
                LightTimer += Time.deltaTime;
               // DoorFlowerLight.SetFloat("_Amount", Mathf.Clamp(LightTimer / OneDuration, 0, 2));
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
                PlayerAnim.SetBool("Staff", false);
                player.enabled = false;
                break;

            //解謎成功
            case state.DoorWin:
                CrackHDRTimer += Time.deltaTime;
                DoorCrackHDR.SetFloat("_ColorAmount", Mathf.Clamp(CrackHDRTimer / TwoDuration, 0, 5));
                bloom.SetActive(false);
                if (DoorCrackHDR.GetFloat("_ColorAmount") >= 1)
                {
                    IsWin = IsWin02 = false;
                    StartCoroutine(EnableDoor());
                    ColorAmount = DoorCrackHDR.GetFloat("_ColorAmount");
                }
                break;

            //關掉解謎門
            case state.DoorOver:
                CrackHDRTimer += Time.deltaTime;
                DoorCrackHDR.SetFloat("_ColorAmount", ColorAmount - CrackHDRTimer / OneDuration);
                CanvasGroupDissloveTimer += Time.deltaTime;
                DoorCanvasGroup.alpha = 1 - CanvasGroupDissloveTimer / TwoDuration;
                DoorFlowerLight.SetFloat("_Amount", 0);
                IsWin = IsWin02 = false;
                Door.SetActive(false);
                DoorFlower.SetActive(false);
                DoorOpen.SetActive(true);
                Camera.main.orthographic = false;
                player.enabled = true;
                DoorWinCollider.enabled = true;
                StartCoroutine(DoorOpenAnim());
                break;
        }
    }
    IEnumerator DoorOpenAnim()
    {
        yield return new WaitForSeconds(2f);
        GameState = state.NONE;
        Dooranim.Play();
    }

    IEnumerator EnableDoor()
    {
        yield return new WaitForSeconds(1f);
        DoorCollider.enabled=false;
        CrackHDRTimer = 0;
        GameState = state.DoorOver;
    }

    void MobileTouch()
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
                    StartCoroutine(TurnLeftMoveUI());
                    GameState = state.STOP;
                }
                else if (GameState == state.LeftMove)
                {
                    TeachUI.enabled = false;
                    LeftMoveUI.SetActive(false);
                    StartCoroutine(TurnJumpMoveUI());
                    GameState = state.STOP;
                }
                else if (GameState == state.JumpMove)
                {
                    TeachUI.enabled = false;
                    JumpMoveUI.SetActive(false);
                    StartCoroutine(TurnSlideMoveUI());
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
    }

    IEnumerator TurnRightMoveUI()
    {
        yield return new WaitForSeconds(1);
        GameState = state.RightMove;
    }

    IEnumerator TurnLeftMoveUI()
    {
        yield return new WaitForSeconds(4);
        GameState = state.LeftMove;
    }

    IEnumerator TurnJumpMoveUI()
    {
        yield return new WaitForSeconds(4);
        GameState = state.JumpMove;
    }

    IEnumerator TurnSlideMoveUI()
    {
        yield return new WaitForSeconds(4);
        GameState = state.SlideMove;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        SkipUI.enabled = true;
        Time.timeScale = 0;
    }

    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(1f);
        enemyAI.enabled = true;
        TeachUI.enabled = true;
        EnemyUI.SetActive(true);
        Time.timeScale = 0;
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

        else if (player.isObstacle == true && !isEnemyAppearUI&& player.hit2.collider.gameObject.tag == "EnemyAppearCollider")
        {
            GameState = state.EnemyAppearUI;
            isEnemyAppearUI = true;
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        TeachUI.enabled = false;
        EnemyUI.SetActive(false);
        if (GameState == state.FinishDrawUI)
        {
            drawEnemy.DrawObject.SetActive(true);
        }
    }

    public void SkipButton()
    {
        TeachObject.SetActive(false);
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        SkipUI.enabled = false;
        GetStickUI.enabled = true;
    }

    public void CloseNoStickUI()
    {
        Time.timeScale = 1;
        GetStickUI.enabled = false;
        player.enabled = true;
    }

    public void NoSkipButton()
    {
        Time.timeScale = 1;
        StartCoroutine(TurnRightMoveUI());
        SkipUI.enabled = false;
    }

    /* void EnemyComingFlashing(int Number,float Seconds)
     {
         StartCoroutine(Flashing(Number, Seconds));
     }
     IEnumerator Flashing(int Number, float Seconds)
     {
         for (int i=0; i< Number*2; i++)
         {
             yield return new WaitForSeconds(Seconds);
         }
     }*/
}
