using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;//GameControllerLV1程式

    bool MirrorIsOpen;
    bool isTouch;

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
        else if(gameController.player.isObstacle == true && gameController.isWin&&!MirrorIsOpen&&!isTouch)
        {
            isTouch = true;
            gameController.player.isCanMove = false;
            MirrorLook.SetActive(true);
            MirrorLookAnim.SetBool("Look", true);
            StartCoroutine(OpenMirror());
        }
        else if (gameController.player.isObstacle == true && gameController.isWin && MirrorIsOpen&&!isTouch)
        {
            isTouch = true;
            StartCoroutine(CloseMirror());
            MirrorLookAnim.SetBool("Look", false);
            MirrorIsOpen = !MirrorIsOpen;
        }
    }
    IEnumerator OpenMirror()
    {
        yield return new WaitForSeconds(1f);
        MirrorIsOpen = !MirrorIsOpen;
        isTouch = false;
    }

    IEnumerator CloseMirror()
    {
        yield return new WaitForSeconds(1f);
        MirrorLook.SetActive(false);
        gameController.player.isCanMove = true;
        isTouch = false;
    }
}
