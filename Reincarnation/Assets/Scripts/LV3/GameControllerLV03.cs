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
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -18)
        {
            virtualCamera.Follow = null;
            //virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 2;
        }
        if (player.transform.position.y <= -30)
        {
            BlackAnim.SetTrigger("FadeOut");
        }

        if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV3");
        }
    }
}
