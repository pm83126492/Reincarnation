using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MirrorTouch : MonoBehaviour
{
    public GameControllerLv1 gameController;
    public PlayableDirector playableDirector;
    private void OnMouseDown()
    {
       // CameraShakeAnim.enabled = true;
        playableDirector.Play();

        StartCoroutine(EffectPlay());   
    }

    IEnumerator EffectPlay()
    {
        yield return new WaitForSeconds(3f);
        
        if (gameController.player.isObstacle == true)
        {
            playableDirector.Pause();
            gameController.MirrorCanvasGroup.gameObject.SetActive(true);
            gameController.GameState = GameControllerLv1.state.MIRROR;
        }
    }
}
