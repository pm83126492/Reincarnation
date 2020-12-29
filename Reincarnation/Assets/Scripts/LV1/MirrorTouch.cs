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
    public AudioClip ScreamAudio;
    bool isParse;
    public GameObject MirrorCam,Spell,BloodText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        // CameraShakeAnim.enabled = true;
        if (gameController.player.isObstacle == true)
        {
            audioSource.PlayOneShot(ScreamAudio);
            Time.timeScale = 3f;
            MirrorCam.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Play());
        }
    }

    private void Update()
    {
        if (float.Parse(playableDirector.time.ToString("0.0")) == 3f&& !isParse)
        {
            Spell.SetActive(false);
            audioSource.PlayOneShot(BrokeGlassAudio);
            playableDirector.Pause();
            _explodable.Explosion();
            StartCoroutine(EffectPlay());
            isParse = true;
        }
        
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(2f);
        //cam01.SetActive(false);
        // cam02.SetActive(true);
        // yield return new WaitForSeconds(1.5f);
        // cam02.SetActive(false);
        Time.timeScale = 1;

        BloodText.SetActive(true);
        yield return new WaitForSeconds(3f);
        MirrorCam.SetActive(false);
        yield return new WaitForSeconds(2f);
        playableDirector.Play();
    }

    IEnumerator EffectPlay()
    {
        yield return new WaitForSeconds(3f);
   
         gameController.MirrorCanvasGroup.gameObject.SetActive(true);
         gameController.GameState = GameControllerLv1.state.MIRROR;
    }
}
