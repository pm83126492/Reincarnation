using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helldoor : MonoBehaviour
{
    public bool DoorIsOpen;
    public CanvasGroup openBackgroundImageCanvasGroup;
    float m_Timer;
    public float fadeDuration = 1f;
    private void Update()
    {
         Physics2D.IgnoreLayerCollision(10, 5);
         
    }
    private void OnMouseDown()
    {
        DoorIsOpen = true;
        GameObject.Find("GameControllerLV0").SendMessage("DoorCanOpen");
    }
}
