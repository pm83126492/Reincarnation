using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyStart : MonoBehaviour
{
    public void ChangeScene()
    {
        FindObjectOfType<Loading>().LoadScene("StartUI");
    }
}
