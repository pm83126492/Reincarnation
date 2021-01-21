using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;//GameControllerLV1程式

    bool MirrorIsOpen;

    public GameObject MirrorLook;
    public GameObject MirrorStory;
    public GameObject MirrorOriginal;

    public Animator MirrorLookAnim;

    private void OnMouseDown()
    {    
        if (gameController.player.isObstacle == true&& !gameController.isWin)
        {
            gameController.TouchMirror();
        }
        else if(gameController.player.isObstacle == true && gameController.isWin&&!MirrorIsOpen)
        {
            gameController.player.isCanMove = false;
            MirrorLook.SetActive(true);
            //MirrorStory.SetActive(false);
            //MirrorOriginal.SetActive(false);
            MirrorLookAnim.SetBool("Look", true);
            MirrorIsOpen = !MirrorIsOpen;
        }
        else if (gameController.player.isObstacle == true && gameController.isWin && MirrorIsOpen)
        {
            StartCoroutine(OpenMirror());
            MirrorLookAnim.SetBool("Look", false);
            MirrorIsOpen = !MirrorIsOpen;
        }
    }

    IEnumerator OpenMirror()
    {
        yield return new WaitForSeconds(1f);
        MirrorLook.SetActive(false);
        gameController.player.isCanMove = true;
        // MirrorStory.SetActive(true);
        // MirrorOriginal.SetActive(true);
    }
}
