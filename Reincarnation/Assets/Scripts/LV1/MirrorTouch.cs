using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;
    private void OnMouseDown()
    {
        gameController.MirrorCanOpen();
    }
}
