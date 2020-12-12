using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;
    public Animator CameraShakeAnim, RingAnim;
    public ParticleSystem RingEffect;
    public PlayableDirector playableDirector;
    private void OnMouseDown()
    {
       // CameraShakeAnim.enabled = true;
        playableDirector.Play();

        //StartCoroutine(EffectPlay());   
    }

    IEnumerator EffectPlay()
    {
        yield return new WaitForSeconds(4f);
        
        if (gameController.player.isObstacle == true)
        {
            gameController.MirrorCanvasGroup.gameObject.SetActive(true);
            gameController.GameState = GameControllerLv1.state.MIRROR;
        }
    }
}
