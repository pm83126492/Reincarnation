using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    public GameControllerLV0 gameController;

    private void Update()
    {
        //判斷門栓勝利條件
        if (transform.localPosition.x >= 420)
        {
            gameController.IsWin = true;
        }

        if(transform.localPosition.x <= -380)
        {
            gameController.IsWin02 = true;
        }
    }
}
