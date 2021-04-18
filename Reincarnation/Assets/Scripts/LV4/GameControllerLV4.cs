using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameControllerLV4 : MonoBehaviour
{
    private Player player;
    public GameObject RebirthPoint, RebirthPoint2;
    public GameObject RebirthCollider, RebirthCollider2;

    public GhostControllder GhostObjects;
    public Animator BlackAnim;

    public CinemachineVirtualCamera virtualCamera;//攝影機
    // Start is called before the first frame update
    void Start()
    {
        if (!StartUI.isAfterStartUI)
        {
            BGMSlider.BGMVoloume = 0.7f;
            AudioSlider.AudioVoloume = 1;
        }

        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.75f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneHeight = 0f;
        player = GameObject.Find("Player").GetComponent<Player>();
        SceneSingleton._Instance.SetState(0);
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
            SceneSingleton.Instance.m_RebirthNumber = 0;
        }

        if (SceneSingleton.Instance.m_RebirthNumber == 1)
        {
            player.transform.position = RebirthPoint.transform.position;
            Destroy(RebirthCollider);
        }
        else if (SceneSingleton.Instance.m_RebirthNumber == 2)
        {
            player.transform.position = RebirthPoint2.transform.position;
            Destroy(RebirthCollider);
            Destroy(RebirthCollider2);
        }
        else if (SceneSingleton.Instance.m_RebirthNumber >= 3)
        {
            SceneSingleton.Instance.m_RebirthNumber = 4;
            player.transform.position = RebirthPoint2.transform.position;
            GhostObjects.transform.parent.gameObject.SetActive(false);
            Destroy(RebirthCollider);
            Destroy(RebirthCollider2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rebirth();
        CanGoLV5();

        if (GhostObjects.isGhostAttackDie)
        {
            BlackAnim.SetTrigger("FadeOut");
            StartCoroutine(RebirthEvent());
        }

        if (GhostObjects.GhostIsOut)
        {
            SceneSingleton.Instance.m_RebirthNumber = 3;
        }
    }

    void CanGoLV5()
    {
        if (player.transform.position.x >= 258)
        {
            SceneSingleton._Instance.SetState(1);
            player.isCanMove = false;
        }
    }

    void Rebirth()
    {
        if (player.isObstacle && player.hit2.collider.gameObject.tag == "Rebirth")
        {
            SceneSingleton.Instance.m_RebirthNumber++;
            Destroy(player.hit2.collider.gameObject);
        }

    }

    //重生倒數
    IEnumerator RebirthEvent()
    {
        yield return new WaitForSeconds(1.5f);
        SceneSingleton._Instance.SetState(2);
    }
}
