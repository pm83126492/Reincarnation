using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Image[] HaloBG;
    [SerializeField]
    private string sceneToLoad;

    public GameObject AudioSettingCanvas;


    private void Start()
    {
        //AudioSettingCanvas.SetActive(false);
        BGMSlider.BGMVoloume = 0.5f;
        AudioSlider.AudioVoloume = 1;
    }

    public void StartGame()
    {
        HaloBG[0].enabled = true;
        StartCoroutine(ChangeScene());
    }

    public void AudioSetting()
    {
        HaloBG[1].enabled = true;
        StartCoroutine(AduioSettingOpen());
    }

    public void BackButton()
    {
        HaloBG[1].enabled = false;
        AudioSettingCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        HaloBG[2].enabled = true;
        Application.Quit();
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<Loading>().LoadScene(sceneToLoad);
    }
    IEnumerator AduioSettingOpen()
    {
        yield return new WaitForSeconds(0.2f);
        AudioSettingCanvas.SetActive(true);
    }
}
