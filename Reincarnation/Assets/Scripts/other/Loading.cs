using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation operation;
    public Canvas Firecanvas;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        Firecanvas.gameObject.SetActive(true);
        StartCoroutine(BeginLoad(sceneName));
    }
    
    private IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        operation = null;
        Firecanvas.gameObject.SetActive(false);
    }
}
