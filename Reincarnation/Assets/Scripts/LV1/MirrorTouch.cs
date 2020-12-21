using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Explodable))]
public class MirrorTouch : MonoBehaviour
{
    public ExplodeOnClick _explodable;
    public GameControllerLv1 gameController;
    public PlayableDirector playableDirector;

    public AudioSource audioSource;
    public AudioClip BrokeGlassAudio;
    bool isParse;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        // CameraShakeAnim.enabled = true;
        if (gameController.player.isObstacle == true)
        {
            playableDirector.Play();
           // StartCoroutine(EffectPlay());
        }
    }

    private void Update()
    {
        if (float.Parse(playableDirector.time.ToString("0.0")) == 3f&& !isParse)
        {
            audioSource.PlayOneShot(BrokeGlassAudio);
            playableDirector.Pause();
            _explodable.Explosion();
            StartCoroutine(EffectPlay());
            isParse = true;
        }
        
    }

    IEnumerator EffectPlay()
    {
        yield return new WaitForSeconds(3f);
   
         gameController.MirrorCanvasGroup.gameObject.SetActive(true);
         gameController.GameState = GameControllerLv1.state.MIRROR;

    }
}
