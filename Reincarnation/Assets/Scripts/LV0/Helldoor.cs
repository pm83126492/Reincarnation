using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helldoor : MonoBehaviour
{
    public bool DoorIsOpen;
    private void Start()
    {
         Physics2D.IgnoreLayerCollision(10, 5);
    }

    //點擊門進入解謎
    private void OnMouseDown()
    {
        DoorIsOpen = true;
        GameObject.Find("GameControllerLV0").SendMessage("DoorCanOpen");
    }
}
