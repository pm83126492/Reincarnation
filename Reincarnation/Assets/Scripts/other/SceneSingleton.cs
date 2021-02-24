using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSingleton : MonoBehaviour
{
    private int m_GameState;//當前狀態 Ex.過關Or失敗
    public int m_RebirthNumber;//重生點號碼

    public static SceneSingleton _Instance { get; private set; }

    private Animator BlackAnim;
    private Player player;
    private BlackFade blackFade;
    public static SceneSingleton Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = new GameObject("SceneSingleton");
                _Instance = go.AddComponent<SceneSingleton>();
            }
            return _Instance;
        }
    }

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetRebirthNumber(int RebirthNumber)
    {
        m_RebirthNumber = RebirthNumber;
    }

    public void SetState(int GameState)
    {
        m_GameState = GameState;
    }

    private void Update()
    {
        FindObject();//尋找物件
        ReloadScence();//重新場景
        ChangeScence();//改變場景
    }

    void FindObject()
    {
        if (BlackAnim == null)
        {
            blackFade = GameObject.Find("FadeCanvas").GetComponentInChildren<BlackFade>();
            BlackAnim = GameObject.Find("FadeCanvas").GetComponentInChildren<Animator>();
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }

    void ChangeScence()
    {
        if (m_GameState == 1)
        {
            BlackAnim.SetTrigger("FadeOut");
            if (blackFade.CanChangeScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }
        }
    }

    void ReloadScence()
    {
        if (m_GameState == 2)
        {
            BlackAnim.SetTrigger("FadeOut");
            player.isCanMove = false;
            player.rigidbody2D.velocity = Vector2.zero;
            if (blackFade.CanChangeScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
