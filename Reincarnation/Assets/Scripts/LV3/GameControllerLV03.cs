using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameControllerLV03 : MonoBehaviour
{
    public Player player;
    public Animator BlackAnim;
    public BlackFade blackFade;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject PlayerCirminalHand;
    public GameObject criminalBody01, criminalBody02, criminalBody03, criminalBody04;
    public GameObject RockFloor;
    public SpriteRenderer criminal01, criminal02, criminal03, criminal04;
    public Sprite[] criminal_hand;
    public PlayerLV3 playerLV3;
    bool isDown;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
        if (IntrodutionUI.SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            IntrodutionUI.isNotOnce = false;
            IntrodutionUI.SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -18|| player.transform.position.x >= 35)
        {
            virtualCamera.Follow = null;
            //virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2;
        }
        if (player.transform.position.y <= -30)
        {
            BlackAnim.SetTrigger("FadeOut");
        }
        if (player.transform.position.y >= -12)
        {
            RockFloor.transform.position = new Vector3(100, 100, 100);
        }

        if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV3");
        }

        BreakHand();
    }

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

    IEnumerator BreakHandTime()
    {
        yield return new WaitForSeconds(5f);
        if (playerLV3.hit2.collider.gameObject.tag == "Swing")
        {
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[0];
            criminalBody01.SetActive(true);
            criminal01.enabled = false;
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing2")
        {
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[1];
            criminalBody02.SetActive(true);
            criminal02.enabled = false;
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing3")
        {
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[2];
            criminalBody03.SetActive(true);
            criminal03.enabled = false;
        }
        else if (playerLV3.hit2.collider.gameObject.tag == "Swing4")
        {
            PlayerCirminalHand.GetComponent<SpriteRenderer>().sprite = criminal_hand[3];
            criminalBody04.SetActive(true);
            criminal04.enabled = false;
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
