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

    public void StartGame()
    {
        HaloBG[0].enabled = true;
        FindObjectOfType<Loading>().LoadScene(sceneToLoad);
        //SceneManager.LoadScene("LV0");
    }

    public void ExitGame()
    {
        HaloBG[2].enabled = true;
        Application.Quit();
    }

}
