using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public Text text;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    bool isNext;
    bool isAudio;

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        isNext = false;
        if (index == 2&&!isAudio)
        {
            audio.Play();
            text.text = "";
            isAudio = true;
            StartCoroutine(MomText());
        }
        else if (index < sentences.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(Type());
        }
        else
        {
            text.text = "";
            FindObjectOfType<Loading>().LoadScene("ReadyStart");
        }
    }

    private void Update()
    {
        if (text.text == sentences[index]) 
        {
            isNext = true;
        }

        if (Input.GetMouseButtonDown(0)&&isNext)
        {
            NextSentence();
        }
    }

    IEnumerator MomText()
    {
        yield return new WaitForSeconds(8f);
        index++;
        text.text = "";
        StartCoroutine(Type());
    }
}
