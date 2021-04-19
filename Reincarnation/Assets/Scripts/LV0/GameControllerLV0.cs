using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;

public class GameControllerLV0 : MonoBehaviour
{
    public Canvas DoorCanvas;//解謎門Canvas
    public Canvas TeachUI;//教學Canvas
    public Canvas SkipUI;//跳過教學Canvas
    public Canvas GetStickUI;//獲得禪杖Canvas
    public Canvas ButtonCanvas;//移動ButtonCavcas

    public CanvasGroup DoorCanvasGroup;//解謎門CanvasGroup
    public CanvasGroup DoorCicleFlowerCanvasGroup;//解謎門花圈CanvasGroup

    public bool IsWin;//上門栓判斷
    public bool IsWin02;//下門栓判斷
    bool isUseObjUI;//教學使用物件中
    bool isDoorAudio;//開門音效播放中

    public PlayerLV0 player;//player程式
    public BlackFade blackFade;//黑頻程式
    public GhostControllder ghostControllder;//Ghost程式
    public Line line;//符畫線程式
    public PlayableDirector playableDirector;//TimeLine
    public CinemachineVirtualCamera virtualCamera;//攝影機
    public BoxCollider2D GhostWall;

    public Material DoorCircleLightMaterial;//解謎門花圈Material
    public Material DoorCrackHDR;//門縫發光Material
    public Material DoorFlowerDissolveMaterial;//門花紋DissloveMaterial

    private float ColorAmount;//門縫發光數值
    private float LightTimer;//禪杖發光And解謎門花圈發光時間
    private float DissolveTimer;//門花紋Disslove時間
    private float CanvasGroupTimer;//解謎門淡入淡出數值
    private float OneDuration = 1f;//需要數字1數值
    private float TwoDuration = 2f;//關掉解謎門淡出數值
    float SlideTime;//教學滑行時間;

    public Animation DoorCircleanim;//解謎門花圈動畫
    public Animation DoorUIanim;//解謎門UI動畫
    public Animation Dooranim;//地圖上的解謎門動畫
    public Animator PlayerAnim;//Player動畫   
    public Animator BlockFadeAnim;//黑頻動畫
    public RuntimeAnimatorController trickAnim,NotrickAnim;//有拿禪杖Animator,無拿禪杖Animator

    public GameObject bloom;//Bloom物件
    public GameObject Door;//解謎結束前有門栓Door
    public GameObject DoorFlower;//解謎結束前有封印花紋Door
    public GameObject DoorOpen;//解謎結束後無封印花紋與門栓Door
    public GameObject EnemyUI;//教學有鬼差UI
    public GameObject DrawUI;//教學畫符UI
    public GameObject GhostAttackUI;//教學被鬼差攻擊UI
    public GameObject TeachObject;//教學關使用到的物件
    public GameObject Door_CorkRed_Under;//解謎門UI下門栓
    public GameObject[] Door_LR;//解謎門UI左邊門
    public GameObject[] Doorlock_LR;//解謎門擋住紅木栓的小木栓
    public GameObject GhostObject;//鬼差物件
    public GameObject Ghost;//鬼差總物件

    public BoxCollider2D DoorCollider;//地圖上解謎門碰撞器
    public BoxCollider2D UIInvisibleWall;//未通過教學檔角色碰撞器
    public BoxCollider2D DoorWinCollider;//通關碰撞器
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
        DoorWinCollider.enabled = false;
        DoorCircleLightMaterial.SetFloat("_OutlineThickness", 0);
        DoorFlowerDissolveMaterial.SetFloat("_DissolveAmount", 0);
        DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(0, 0, 0, 0));
        DoorCrackHDR.SetFloat("_ColorAmount", 0);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.72f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 2f;

        if (!StartUI.isAfterStartUI)
        {
            BGMSlider.BGMVoloume = 0.7f;
            AudioSlider.AudioVoloume = 1;
        }
    }

    void Update()
    {
        PlayableTime();//教學關TimeLine時間

        DoorState();//關卡門狀態機

        UIEvent();//教學關使用物件碰撞

        //解謎成功
        if (IsWin == true && IsWin02 == true)
        {
            GameState = state.DoorWin;
        }

        /*if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV1");
        }*/
    }

    void DoorState()
    {
        switch (GameState)
        {
            case state.NONE:
                break;

            case state.STOP:
                player.anim.SetFloat("WalkSpeed", 0);
                player.anim.SetBool("Slide", false);
                player.obstacle = null;
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
                   // Ghost.SetActive(true);
                    GameState = state.EnemyAppearUI;
                }
                break;

            case state.EnemyAppearUI:
                GhostWall.enabled = false;
                player.isCanMove = true;
                player.anim.SetBool("SquatPush", false);
                if (player.isObstacle == true && player.hit2.collider.gameObject.tag == "EnemyAppearCollider")
                {
                    ghostControllder.enabled = true;
                    StartCoroutine(PauseGame());
                    GameState = state.DrawAppearUI;
                }

                break;

            case state.DrawAppearUI:
                if (ghostControllder.SignCanvasGroup.alpha >= 0.9f)
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
                    GameState = state.DrawAppearUI;
                    ghostControllder.isPlayGhostComeAudio = false;
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
                if (DissolveTimer == 0)
                {
                    AudioManager.Instance.PlaySource("Dissolve","0");
                }
                DissolveTimer += Time.deltaTime;
                DoorFlowerDissolveMaterial.SetFloat("_DissolveAmount", Mathf.Clamp(DissolveTimer / OneDuration, 0, 1.1f));
                DoorCicleFlowerCanvasGroup.alpha = 1 - DissolveTimer / OneDuration;
                Camera.main.orthographic = true;
                player.isCanMove = false;
                //player.enabled = false;
                break;

            //解謎成功
            case state.DoorWin:
                if (!isDoorAudio)
                {
                    AudioManager.Instance.PlaySource("DoorOpen", "0");
                    isDoorAudio = true;
                }
                Camera.main.orthographic = false;
                PlayerAnim.SetBool("Staff", false);
                DoorCrackHDR.SetFloat("_ColorAmount", 1.5f);
                DoorUIanim.Play();
                StartCoroutine(DoorOpenAnim());
                ColorAmount = DoorCrackHDR.GetFloat("_ColorAmount");
                Door_CorkRed_Under.transform.parent = Door_LR[0].transform;
                Doorlock_LR[0].transform.parent = Door_LR[0].transform;
                Doorlock_LR[1].transform.parent = Door_LR[1].transform;
                CanvasGroupTimer = 0;
                break;

            //關掉解謎門
            case state.DoorOver:
                CanvasGroupTimer += Time.deltaTime;
                DoorCanvasGroup.alpha = 1 - CanvasGroupTimer / TwoDuration;
                IsWin = IsWin02 = false;
                Door.SetActive(false);
                DoorFlower.SetActive(false);
                DoorOpen.SetActive(true);
                player.isCanMove = true;
                DoorWinCollider.enabled = true;
                if (DoorCanvasGroup.alpha <= 0)
                {
                    DoorCanvas.enabled = false;
                    bloom.SetActive(false);
                    GameState = state.NONE;
                }
                break;
        }
    }

    //TimeLine指定秒數事件判斷
    void PlayableTime()
    {
        if (float.Parse(playableDirector.time.ToString("0.0")) == 4f)
        {
            playableDirector.Pause();
            GameState = state.RightMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 5f)
        {
            playableDirector.Pause();
            GameState = state.LeftMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 9f)
        {
            playableDirector.Pause();
            GameState = state.JumpMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 13f)
        {
            playableDirector.Pause();
            GameState = state.SlideMove;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 14f)
        {
            playableDirector.Pause();
            GameState = state.PleaseObj;
        }
        else if (float.Parse(playableDirector.time.ToString("0.0")) == 17f)
        {
            playableDirector.Pause();
            GameState = state.UseObj;
        }
    }

    //教學關使用物件碰撞
    void UIEvent()
    {
        if (player.isObstacle == true && !isUseObjUI && player.hit2.collider.gameObject.tag == "smallobstacle")
        {
            GameState = state.UseObj;
            isUseObjUI = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneSingleton.Instance.SetState(1);
            //BlockFadeAnim.SetTrigger("FadeOut");
        }
    }



    /// <summary>
    /// 倒數事件
    /// </summary>
    /// <returns></returns>

    //解謎成功動畫倒數
    IEnumerator DoorOpenAnim()
    {
        yield return new WaitForSeconds(2f);
        Dooranim.Play();
        yield return new WaitForSeconds(1f);
        DoorCollider.enabled = false;
        GameState = state.DoorOver;
    }

    //轉換為Dissolve倒數
    IEnumerator BoolDoorFlowerDissolve()
    {
        yield return new WaitForSeconds(3);
        GameState = state.DoorDissolve;
    }


    //跳過UI出現倒數
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        SkipUI.enabled = true;
        Time.timeScale = 0;
    }

    //遊戲暫停倒數
    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(1f);
        ghostControllder.GhostAI.enabled = true;
        EnemyUI.SetActive(true);
        Time.timeScale = 0;
        ghostControllder.audioSource.Pause();
    }

    //鬼差UI出現倒數
    IEnumerator GhostAttack()
    {
        yield return new WaitForSeconds(1f);
        GhostAttackUI.SetActive(true);
        Time.timeScale = 0;
    }

    //獲得禪杖UI出現倒數
    IEnumerator OpenGetStickUI()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        GetStickUI.enabled = true;
    }


    /// <summary>
    /// 點擊事件
    /// </summary>

    //點擊地圖門
    public void DoorCanOpen()
    {
        if (player.isObstacle == true)
        {
            DoorCanvas.enabled = true;
            GameState = state.DoorLightFadeIn;
        }
    }

    //點擊解謎門花圈
    public void TouchFlowerCircle()
    {
        if (DoorCanvasGroup.alpha >= 1 && DoorCircleLightMaterial.GetFloat("_OutlineThickness") <= 0)
        {
            DoorCircleanim.Play();
            GameState = state.DoorCanAnim;
            StartCoroutine(BoolDoorFlowerDissolve());
            DoorCircleLightMaterial.SetColor("_OutlineColor", new Vector4(255, 100, 0, 255) * 0.004f);
            LightTimer = 0;
        }
    }

    //點擊繼續遊戲
    public void ContinueGame()
    {
        Time.timeScale = 1;
        TeachUI.enabled = false;
    }

    //點擊跳過新手教學
    public void SkipButton()
    {
        //Time.timeScale = 1;
        PlayerAnim.runtimeAnimatorController = trickAnim as RuntimeAnimatorController;
        SkipUI.enabled = false;
        GetStickUI.enabled = true;
        ButtonCanvas.enabled = true;
        Ghost.SetActive(false);
        //player.isCanMove = true;
    }

    //點擊不跳過新手教學
    public void NoSkipButton()
    {
        Time.timeScale = 1;
        TeachUI.enabled = true;
        ButtonCanvas.enabled = true;
        playableDirector.Play();
        GameState = state.STOP;
        SkipUI.enabled = false;
    }

    //點擊關閉獲得禪杖UI
    public void CloseGetStickUI()
    {
        Time.timeScale = 1;
        GetStickUI.enabled = false;
        player.isCanMove = true;
    }

    //點擊鬼差UI繼續
    public void EnemyUIContinue()
    {
        EnemyUI.SetActive(false);
        ghostControllder.audioSource.Play();
        Time.timeScale = 1;
    }

    //點擊畫符UI繼續
    public void DrawUIContinue()
    {
        DrawUI.SetActive(false);
        ghostControllder.audioSource.Play();
        Time.timeScale = 1;
    }

    //點擊被鬼差攻擊UI繼續
    public void GhostAttackUIContinue()
    {
        ghostControllder.vignette.color.value = new Color(0f, 0f, 0f);
        GhostObject.transform.position = new Vector3(19.4f, GhostObject.transform.position.y, GhostObject.transform.position.z);
        ghostControllder.GhostState = GhostControllder.State.COMING;
        if (ghostControllder.isWhiteGhost)
        {
            ghostControllder.GhostWhiteAnim.SetBool("TongueAttack", false);
            player.anim.SetBool("GhostWhiteAttack", false);
        }
        else if (ghostControllder.isBlackGhost)
        {
            ghostControllder.GhostBlackAnim.SetBool("ChainAttack", false);
            player.anim.SetBool("GhostBlackAttack", false);
        }
        ghostControllder.isDrawUI = false;
        ghostControllder.isGhostAttackDie = false;
        ghostControllder.SignAppearTime = 0;
        LineCollider.ColliderNumber = 0;
        GhostAttackUI.SetActive(false);
        for (int i = 0; i< line.TrailList.Count; i++)
        {
            Destroy(line.TrailList[i]);
            if(i== line.TrailList.Count-1)
            {
                line.TrailList.Clear();
            }
        }
        Time.timeScale = 1;
    }
}
