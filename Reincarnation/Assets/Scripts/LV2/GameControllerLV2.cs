using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameControllerLV2 : MonoBehaviour
{
    public GameObject IceOrgan;//起重機冰塊物件
    public GameObject IceButtonBox;//推_小冰塊物件
    public GameObject IcePlate;//過關平台冰塊物件
    public GameObject Player;//玩家物件
    public GameObject Icelittle;//小冰塊物件
    public GameObject RebirthPoint;//玩家重生點1
    public GameObject RebirthPoint2;//玩家重生點2
    public GameObject RebirthCollider;//玩家重生點碰撞器
    public GameObject RebirthCollider2;//玩家重生點碰撞器2

    public GhostControllder GhostObjects;//鬼差物件

    public ParticleSystem SmokeIce01, SmokeIce02, SmokeIce03;//冰霧特效

    public Transform CanButtonPoint;//按鈕地洞位置
    public Transform CanButtonPoint02;//按鈕地洞位置2
    public Transform PlayerButtonPoint;//玩家可按鈕地洞位置
    public Transform PlayerButtonPoint02;//玩家可按鈕地洞位置2
    public Transform IcePlateStop;//過關平台冰塊停止位置
    public Transform IcePlateStart;//過關平台冰塊開始位置

    public Animator BlackAnim,PlayerAnim;//黑頻動畫,角色動畫

    public SmokeParticle smokeParticle01, smokeParticle02, smokeParticle03;//冰霧程式
    public PlayerLV2 player;//Player程式
    public BlackFade blackFade;//黑頻程式

    public Collider2D collider2d,collider2d_hole;//按鈕地洞碰撞器_無洞,按鈕地洞碰撞器_有洞

    public CinemachineVirtualCamera virtualCamera;//攝影機

    bool isWin;//過關中

    public static int LV2RebirthNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (!StartUI.isAfterStartUI)
        {
            BGMSlider.BGMVoloume = 0.7f;
            AudioSlider.AudioVoloume = 1;
        }

        // GhostObjects = GameObject.Find("Ghost");
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.72f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 2f;
        SceneSingleton._Instance.SetState(0);
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }
        isWin = false;
        StartCoroutine(PlaySmokeIce01());
        StartCoroutine(PlaySmokeIce02());
        StartCoroutine(PlaySmokeIce03());

        if (SceneSingleton.Instance.m_RebirthNumber == 1)
        {
            player.transform.position = RebirthPoint.transform.position;
            Destroy(RebirthCollider);
        }
        else if (SceneSingleton.Instance.m_RebirthNumber == 2)
        {
            //SceneSingleton.Instance.m_RebirthNumber = 3;
            player.transform.position = RebirthPoint2.transform.position;
            Destroy(RebirthCollider);
            Destroy(RebirthCollider2);
        }else if(SceneSingleton.Instance.m_RebirthNumber == 3)
        {
            GhostObjects.transform.parent.gameObject.SetActive(false);
            player.transform.position = RebirthPoint2.transform.position;
            Destroy(RebirthCollider);
            Destroy(RebirthCollider2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CanGoLV3();//切換第三關
        LastIceMove();//過關冰塊平台移動
        Lose();//失敗
        IceColliderHole();//冰塊地板洞Collider控制
        Rebirth();

        if (player.transform.position.y <= -1)
        {
            player.anim.SetTrigger("Down");
        }
    }

    //冰霧噴射時間
    IEnumerator PlaySmokeIce01()
    {
        yield return new WaitForSeconds(2f);
        SmokeIce01.Play();
        SmokeIce01.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);
        StartCoroutine(PlaySmokeIce01());
    }
    IEnumerator PlaySmokeIce02()
    {
        yield return new WaitForSeconds(1f);
        SmokeIce02.Play();
        SmokeIce02.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(4f);
        StartCoroutine(PlaySmokeIce02());
    }
    IEnumerator PlaySmokeIce03()
    {
        yield return new WaitForSeconds(2f);
        SmokeIce03.Play();
        SmokeIce03.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);
        StartCoroutine(PlaySmokeIce03());
    }

    //冰塊地板洞Collider控制
    void IceColliderHole()
    {
        if (Icelittle.transform.localPosition.x >= 105)
        {
            collider2d_hole.enabled = true;
            collider2d.enabled = false;
        }
    }

    //過關冰塊平台移動
    void LastIceMove()
    {
        if (player.transform.position.x >= PlayerButtonPoint.position.x && player.transform.position.x <= PlayerButtonPoint02.position.x)
        {
            if (IcePlate.transform.position.x > IcePlateStop.position.x)
            {
                IcePlate.transform.Translate(-1 * Time.deltaTime, 0, 0);
            }
        }
        else if (IceButtonBox.transform.position.x >= CanButtonPoint.position.x && IceButtonBox.transform.position.x <= CanButtonPoint02.position.x)
        {
            if (IcePlate.transform.position.x > IcePlateStop.position.x)
            {
                IcePlate.transform.Translate(-1 * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if (IcePlate.transform.position.x < IcePlateStart.position.x)
            {
                IcePlate.transform.Translate(1.5f * Time.deltaTime, 0, 0);
            }
        }
    }

    //切換第三關
    void CanGoLV3()
    {
        if (player.transform.position.x >= 130)
        {
            SceneSingleton._Instance.SetState(1);
        }
    }

    //失敗
    void Lose()
    {
        if (Player.transform.position.y <= -20|| GhostObjects.isGhostAttackDie)
        {
            BlackAnim.SetTrigger("FadeOut");
            StartCoroutine(RebirthEvent());
        }


        if (Player.transform.position.y <= -6)
        {
            virtualCamera.Follow = null;
        }

        if (smokeParticle01.isIceSmoke || smokeParticle02.isIceSmoke || smokeParticle03.isIceSmoke)
        {
            PlayerAnim.SetTrigger("IceSmokeDie");
            StartCoroutine(IceSmokeDie());
            player.rigidbody2D.sharedMaterial = null;
            player.enabled = false;
        }
    }

    //重生點
    void Rebirth()
    {
        if (player.isObstacle && player.hit2.collider.gameObject.tag == "Rebirth")
        {
            SceneSingleton.Instance.m_RebirthNumber++;
            Destroy(player.hit2.collider.gameObject);
        }

        if (GhostObjects.GhostIsOut)
        {
            SceneSingleton.Instance.m_RebirthNumber=3;
        }
    }

    //冰霧噴到死亡動畫
    IEnumerator IceSmokeDie()
    {
        yield return new WaitForSeconds(2f);
        BlackAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        SceneSingleton._Instance.SetState(2);
    }

    //重生倒數
    IEnumerator RebirthEvent()
    {
        yield return new WaitForSeconds(1.5f);
        SceneSingleton._Instance.SetState(2);
    }
}
