using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmokeDieAnim : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="DieObjects")
        {
            Debug.Log("OK");
        }
    }
}
