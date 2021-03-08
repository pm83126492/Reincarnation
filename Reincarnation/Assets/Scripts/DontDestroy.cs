using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public int SceneNumber;

    public static DontDestroy _Instance { get; private set; }
    public static DontDestroy Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = new GameObject("BGM");
                _Instance = go.AddComponent<DontDestroy>();
            }
            return _Instance;
        }
    }
    // Start is called before the first frame update
    void Start()
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

    private void Update()
    {
        if (SceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            Destroy(gameObject);
        }
    }
}
