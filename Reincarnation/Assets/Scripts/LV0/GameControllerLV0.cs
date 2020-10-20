using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerLV0 : MonoBehaviour
{
    public Canvas DoorCanvas;
    public Canvas TeachUI;
    public bool IsWin;
    public bool IsWin02;
    bool isUseObjUI;
    public Player player;
    public BlackFade blackFade;

    public CanvasGroup DoorCanvasGroup;
    public CanvasGroup DoorCicleFlowerCanvasGroup;
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
    public Animation anim;
    public Animator PlayerAnim;
    public Animator BlockFadeAnim;
    public Animation Dooranim;

    public GameObject bloom;
    public GameObject DoorEffect;
    public GameObject Door;
    public GameObject DoorFlower;
    public GameObject DoorOpen;
    public GameObject RightMoveUI;
    public GameObject LeftMoveUI;
    public GameObject JumpMoveUI;
    public GameObject SlideMoveUI;
    public GameObject UseObjUI;
    public BoxCollider2D DoorCollider;
    public BoxCollider2D DoorWinCollider;
    public enum state
    {
        NONE,
        STOP,
        RightMove,
        LeftMove,
        JumpMove,
        SlideMove,
        UseObj,
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
        bloom.SetActive(false);
        player.GetComponent<MeshRenderer>().sortingOrder = 40;
        player.enabled = false;
        StartCoroutine(TurnRightMoveUI());
        DoorEffect.GetComponent<MeshRenderer>().sortingOrder = 0;
        Door.SetActive(true);
        DoorFlower.SetActive(true);
        DoorOpen.SetActive(false);
        GameState = state.NONE;
        IsWin = IsWin02 = isUseObjUI = false;
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

        if (player.isObstacle == true&& !isUseObjUI)
        {
            if(player.hit2.collider.gameObject.tag == "obstacle")
            {
                GameState = state.UseObj;
                isUseObjUI = true;
            }
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

            //小圖門花紋發光
            case state.DoorLightFadeIn:
                PlayerAnim.SetBool("Staff", true);
                LightTimer += Time.deltaTime;
               // DoorFlowerLight.SetFloat("_Amount", Mathf.Clamp(LightTimer / OneDuration, 0, 2));
                if (LightTimer >= 3)
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
                CrackHDRTimer += Time.deltaTime;
                DoorCrackHDR.SetFloat("_ColorAmount", Mathf.Clamp(CrackHDRTimer / TwoDuration, 0, 5));
                PlayerAnim.SetBool("Staff", false);
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
        yield return new WaitForSeconds(4);
      //  PlayerAnim.SetTrigger("idle");
      //  player.enabled = false;
        GameState = state.RightMove;
    }

    IEnumerator TurnLeftMoveUI()
    {
        yield return new WaitForSeconds(4);
      //  PlayerAnim.SetTrigger("idle");
      //  player.enabled = false;
        GameState = state.LeftMove;
    }

    IEnumerator TurnJumpMoveUI()
    {
        yield return new WaitForSeconds(4);
       // PlayerAnim.SetTrigger("idle");
       // player.enabled = false;
        GameState = state.JumpMove;
    }

    IEnumerator TurnSlideMoveUI()
    {
        yield return new WaitForSeconds(4);
        // PlayerAnim.SetTrigger("idle");
        // player.enabled = false;
        GameState = state.SlideMove;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BlockFadeAnim.SetTrigger("FadeOut");
        }
    }
}
