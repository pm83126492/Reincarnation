using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    public bool CanChangeScene;

    void Awake()
    {
        CanChangeScene = false;
    }

    public void ChangeScene()
    {
        CanChangeScene = true;
    }
}
