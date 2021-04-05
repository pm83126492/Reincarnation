using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LV5Introdution : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public static bool isNotOnce;
    public static int SceneNubmer;

    float time;

    bool isBeginDisapear;
    public PlayerLV5 player;
    // Start is called before the first frame update
    void Start()
    {
        if (isNotOnce)
        {
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
        else
        {
            player.enabled = false;
            StartCoroutine(DisappearUI());
        }
    }

    private void Update()
    {
        if (isBeginDisapear)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = 1 - time / 1f;
            if (canvasGroup.alpha == 0)
            {
                canvasGroup.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DisappearUI()
    {
        yield return new WaitForSeconds(3f);
        player.enabled = true;
        isNotOnce = true;
        isBeginDisapear = true;
    }
}
