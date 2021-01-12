using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;//GameControllerLV1程式

    private void OnMouseDown()
    {    
        if (gameController.player.isObstacle == true)
        {
            gameController.TouchMirror();
        }
    }
}
