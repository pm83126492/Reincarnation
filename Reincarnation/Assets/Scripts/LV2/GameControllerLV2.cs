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
    public GameObject IceBig;//推_大冰塊物件

    public ParticleSystem SmokeIce01, SmokeIce02, SmokeIce03;//冰霧特效

    public Transform CanButtonPoint;//按鈕地洞位置
    public Transform CanButtonPoint02;//按鈕地洞位置2
    public Transform IcePlateStop;//過關平台冰塊停止位置

    public Animator BlackAnim,PlayerAnim;//黑頻動畫,角色動畫

    public SmokeParticle smokeParticle01, smokeParticle02, smokeParticle03;//冰霧程式
    public PlayerLV2 player;//Player程式
    public BlackFade blackFade;//黑頻程式

    public Collider2D collider2d,collider2d_hole;//按鈕地洞碰撞器_無洞,按鈕地洞碰撞器_有洞

    public CinemachineVirtualCamera virtualCamera;//攝影機

    bool isWin;//過關中

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.72f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 2f;
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }
        isWin = false;
        StartCoroutine(PlaySmokeIce01());
        StartCoroutine(PlaySmokeIce02());
        StartCoroutine(PlaySmokeIce03());
    }

    // Update is called once per frame
    void Update()
    {
        CanGoLV3();//切換第三關
        LastIceMove();//過關冰塊平台移動
        Lose();//失敗
        IceColliderHole();//冰塊地板洞Collider控制

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
        yield return new WaitForSeconds(3f);
        StartCoroutine(PlaySmokeIce01());
    }
    IEnumerator PlaySmokeIce02()
    {
        yield return new WaitForSeconds(1f);
        SmokeIce02.Play();
        yield return new WaitForSeconds(4f);
        StartCoroutine(PlaySmokeIce02());
    }
    IEnumerator PlaySmokeIce03()
    {
        yield return new WaitForSeconds(2f);
        SmokeIce03.Play();
        yield return new WaitForSeconds(3f);
        StartCoroutine(PlaySmokeIce03());
    }

    //冰塊地板洞Collider控制
    void IceColliderHole()
    {
        if (IceBig.transform.localPosition.x >= 111)
        {
            collider2d_hole.enabled = true;
            collider2d.enabled = false;
        }
    }

    //失敗
    void Lose()
    {
        if (Player.transform.position.y <= -150)//|| player.CanChangeScene)
        {
            BlackAnim.SetTrigger("FadeOut");
        }

        if(Player.transform.position.y <= -6)
        {
            virtualCamera.Follow = null;
        }

        if (blackFade.CanChangeScene&&!isWin)
        {
            SceneManager.LoadScene("LV2");
        }

        if(smokeParticle01.isIceSmoke|| smokeParticle02.isIceSmoke || smokeParticle03.isIceSmoke)
        {
            PlayerAnim.SetTrigger("IceSmokeDie");
            StartCoroutine(IceSmokeDie());
            player.rigidbody2D.sharedMaterial = null;
            player.enabled = false;
        }
    }

    //過關冰塊平台移動
    void LastIceMove()
    {
        if (IceOrgan.transform.position.y >= 12)
        {
            //IceOrgan.transform.position = new Vector3(IceOrgan.transform.position.x, 12, IceOrgan.transform.position.z);
        }

        if (IceButtonBox.transform.position.x >= CanButtonPoint.position.x && IceButtonBox.transform.position.x <= CanButtonPoint02.position.x)
        {
            if (IcePlate.transform.position.x > IcePlateStop.position.x)
            {
                IcePlate.transform.Translate(-1 * Time.deltaTime, 0, 0);
            }
        }
    }

    //切換第三關
    void CanGoLV3()
    {
        if (player.transform.position.x >= 125)
        {
            isWin = true;
            BlackAnim.SetTrigger("FadeOut");
        }

        if (blackFade.CanChangeScene && isWin)
        {
            SceneManager.LoadScene("LV3");
        }
    }

    //冰霧噴到死亡動畫
    IEnumerator IceSmokeDie()
    {
        yield return new WaitForSeconds(3f);
        BlackAnim.SetTrigger("FadeOut");
    }
}
