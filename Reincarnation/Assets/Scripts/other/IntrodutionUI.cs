using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntrodutionUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    float time;
    bool isDisappear;
    bool isTouch;
    public Player player;
    public static bool isNotOnce;
    public static int SceneNubmer;
    // Start is called before the first frame update
    void Start()
    {
        /*if (SceneNubmer != SceneManager.GetActiveScene().buildIndex)
        {
            isNotOnce = false;
            SceneNubmer = SceneManager.GetActiveScene().buildIndex;
        }*/

        if (isNotOnce)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            player.enabled = false;
            StartCoroutine(DisappearUI());
        }
    }

    private void Update()
    {
        if (isTouch)
        {
            player.enabled = true;
            time += Time.deltaTime;
            canvasGroup.alpha = 1 - time / 1f;
            if (canvasGroup.alpha == 0)
            {
                canvasGroup.gameObject.SetActive(false);
            }
            isNotOnce = true;
        }
    }

    public void OnTouch()
    {
        if (isDisappear == true)
        {
            isTouch = true;
        }
    }

    IEnumerator DisappearUI()
    {
        yield return new WaitForSeconds(5f);
        isDisappear = true;
    }

}
