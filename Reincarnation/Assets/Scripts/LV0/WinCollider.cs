using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    public GameControllerLV0 gameController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinCollider01"))
        {
            gameController.IsWin =true;
        }

        if (collision.CompareTag("WinCollider02"))
        {
            gameController.IsWin02 = true;
        }
    }
}
