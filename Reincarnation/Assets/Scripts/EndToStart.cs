using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndToStart : MonoBehaviour
{
    void Start()
    {
        SceneSingleton._Instance.SetState(0);
        Time.timeScale = 1;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(2);
        SceneSingleton.Instance.m_RebirthNumber = 0;
        LV5Introdution.isNotOnce = false;
    }
}
