using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

[RequireComponent(typeof(Explodable))]
public class MirrorTouch : MonoBehaviour
{
    public ExplodeOnClick _explodable;
    public GameControllerLv1 gameController;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera virtualCamera;

    public AudioSource audioSource;
    public AudioClip BrokeGlassAudio;
    public AudioClip ScreamAudio;
    bool isParse;
    bool isCine;
    float CineTimer;
    public GameObject Spell;
    public GameObject EyesLight;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {    
        if (gameController.player.isObstacle == true)
        {
            gameController.TouchMirror();
        }
    }
}
