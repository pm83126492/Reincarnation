using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameControllerLV03 : MonoBehaviour
{
    public Animator BlackAnim;//黑頻動畫

    public BlackFade blackFade;//黑頻程式
    public CinemachineVirtualCamera virtualCamera;//攝影機
    public PlayerLV3 playerLV3;//Player程式

    public GameObject PlayerCirminalHand;//Player手上斷手物件
    public GameObject criminalBody01, criminalBody02, criminalBody03, criminalBody04;//斷手身體物件
    public GameObject RockFloor;//岩石物件
    public GameObject RebirthPoint;//玩家重生點1

    public SpriteRenderer criminal01, criminal02, criminal03, criminal04;//罪犯Sprite
    public Sprite[] criminal_hand;//斷手Sprite

    public ParticleSystem[] BloodEffect;//血特效
    bool isDown;//掉落中
    bool isWin;//過關
    bool isUp;//在最上層岩石中

    public static int LV3RebirthNumber;//重生數

    public AudioSource audioSource;//音效
    public AudioClip[] criminaAudio;//罪犯慘叫音效
    // Start is called before the first frame update
    void Start()
    {
        SceneSingleton._Instance.SetState(0);
        // Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.72f;
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            SceneSingleton.Instance.m_RebirthNumber = 0;
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }

        if (SceneSingleton.Instance.m_RebirthNumber==1)
        {
            playerLV3.transform.position = RebirthPoint.transform.position;
        }

        if (!StartUI.isAfterStartUI)
        {
            BGMSlider.BGMVoloume = 0.7f;
            AudioSlider.AudioVoloume = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Lose();//失敗

        BreakHand();//罪犯斷手事件

        CanGoLV4();//前往第四關
    }

    //罪犯斷手事件
    void BreakHand()
    {
        if((playerLV3.isSwingJump&&!playerLV3.isSwingJump2 )||(!playerLV3.isSwingJump && playerLV3.isSwingJump2))
        {
            StartCoroutine(BreakHandTime());           
        }
        else
        {
            StopAllCoroutines();
        }
    }

    void CanGoLV4()
    {
        if (playerLV3.transform.position.x >= 42)
        {
            SceneSingleton._Instance.SetState(1);
            SceneSingleton.Instance.m_RebirthNumber = 0;
        }
    }

    //失敗
    void Lose()
    {
        //墜落攝影機不跟隨
        if (playerLV3.transform.position.y <= -18)
        {
            virtualCamera.Follow = null;
            playerLV3.enabled = false;
        }
        //淡出
        if (playerLV3.transform.position.y <= -70)
        {
            SceneSingleton._Instance.SetState(2);
        }
        //站上最上層岩石
        if (playerLV3.transform.position.y >= -12 && playerLV3.transform.position.x >= -2)
        {
            RockFloor.transform.position = new Vector3(100, 100, 100);
            isUp = true;
            if (LV3RebirthNumber == 0)
            {
                SceneSingleton.Instance.m_RebirthNumber=1;
            }
        }
        //掉落
        if ((isUp && playerLV3.transform.position.y <= -12) || playerLV3.transform.position.y <= -18)
        {
            playerLV3.enabled = false;
            playerLV3.anim.SetTrigger("Down");
        }
    }

    //罪犯斷手時間計時
    IEnumerator BreakHandTime()
    {
        yield return new WaitForSeconds(5f);
        playerLV3.enabled = false;
        if (playerLV3.hit2.collider.gameObject.tag == "Swing")
        {
            BloodEffect[0].Play();
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[0];
            criminalBody01.SetActive(true);
            criminalBody01.GetComponent<AudioSource>().PlayOneShot(criminaAudio[0],AudioSlider.AudioVoloume);
            criminal01.enabled = false;
            StopAllCoroutines();
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing2")
        {
            BloodEffect[1].Play();
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[1];
            criminalBody02.SetActive(true);
            criminalBody02.GetComponent<AudioSource>().PlayOneShot(criminaAudio[1], AudioSlider.AudioVoloume);
            criminal02.enabled = false;
            StopAllCoroutines();
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing3")
        {
            BloodEffect[2].Play();
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[2];
            criminalBody03.SetActive(true);
            criminalBody03.GetComponent<AudioSource>().PlayOneShot(criminaAudio[2], AudioSlider.AudioVoloume);
            criminal03.enabled = false;
            StopAllCoroutines();
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing4")
        {
            BloodEffect[3].Play();
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[3];
            criminalBody04.SetActive(true);
            criminalBody04.GetComponent<AudioSource>().PlayOneShot(criminaAudio[3], AudioSlider.AudioVoloume);
            criminal04.enabled = false;
            StopAllCoroutines();
        }
        PlayerCirminalHand.SetActive(true);
        playerLV3.transform.parent = null;
        playerLV3.rigidbody2D.isKinematic = false;
        if (!isDown)
        {
            if (playerLV3.obstacle2.GetComponent<Rigidbody2D>().angularVelocity > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                playerLV3.rigidbody2D.velocity = new Vector2(4.5f, 0);
            }
            else if (playerLV3.obstacle2.GetComponent<Rigidbody2D>().angularVelocity < 0)
            {
                transform.localRotation = new Quaternion(0, 180, 0, 0);
                playerLV3.rigidbody2D.velocity = new Vector2(-4.5f, 0);
            }
            isDown = true;
        }
    }
}
