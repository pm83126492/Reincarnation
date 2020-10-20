using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerLV03 : MonoBehaviour
{
    public Player player;
    public Animator BlackAnim;
    public BlackFade blackFade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -15)
        {
            BlackAnim.SetTrigger("FadeOut");
        }

        if (blackFade.CanChangeScene)
        {
            SceneManager.LoadScene("LV4");
        }
    }
}
