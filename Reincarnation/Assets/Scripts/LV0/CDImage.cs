using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDImage : MonoBehaviour
{
    public float coldTime;//冷卻時間
    public float timer = 0;//計時器初始值
    private Image filledImage;
    public bool isStartTimer;

    void Start()
    {
        filledImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartTimer)
        {
            timer += Time.deltaTime;
            filledImage.fillAmount = (coldTime - timer) / coldTime;
        }
        if (timer >= coldTime)
        {
            filledImage.fillAmount = 0;
            timer = 0;
            isStartTimer = false;
        }
    }
}
