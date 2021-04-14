using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _Instance { get; private set; }

    public AudioSource audioSource;

    public bool isMute;//是否靜音

    //[SerializeField] AudioClip[] sounds;
    
    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = new GameObject("AudioSingleton");
                _Instance = go.AddComponent<AudioManager>();
            }
            return _Instance;
        }

    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void PlaySource(string sound, string LV)
    {
        if (!isMute)
        {
            AudioClip clip = (AudioClip)Resources.Load("AudioClip/LV"+ LV +"/" + sound, typeof(AudioClip));
            audioSource.PlayOneShot(clip, AudioSlider.AudioVoloume);
        }
    }

    public void CanPausePlaySource(bool CanPause,bool isPause,string sound, string LV)
    {
        audioSource.volume = AudioSlider.AudioVoloume;
        if (!isMute&&!isPause&&!CanPause)
        {
            audioSource.clip = (AudioClip)Resources.Load("AudioClip/LV" + LV + "/" + sound, typeof(AudioClip));
            audioSource.Play();
        }
        else if (!isMute && !isPause && CanPause)
        {
            audioSource.loop = true;
            audioSource.clip = (AudioClip)Resources.Load("AudioClip/LV" + LV + "/" + sound, typeof(AudioClip));
            audioSource.Play();
        }
        else if (!isMute && isPause&&CanPause)
        {
            audioSource.Pause();
        }
        else if (!isMute && isPause && !CanPause)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }
}
