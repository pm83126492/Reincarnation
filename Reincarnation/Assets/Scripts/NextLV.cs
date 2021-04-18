using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLV : MonoBehaviour
{
    bool isTouch;

    public void ChangeNextScene()
    {
        if (!isTouch)
        {
            SceneSingleton.Instance.SetState(1);
            isTouch = true;
        }
    }

    public void BossChangeNextScene()
    {
        if (!isTouch)
        {
            RunnerKingController.WinNumber = 30;
            isTouch = true;
        }
    }
}
