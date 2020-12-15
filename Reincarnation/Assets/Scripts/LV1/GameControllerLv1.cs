using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GameControllerLv1 : MonoBehaviour
{
    public MirrorTouch mirrorTouch;
    public CanvasGroup MirrorCanvasGroup;
    public Player player;
    public Transform MidPoint;
    float CanvasGroupTimer;

    public UniversalRenderPipelineAsset cameraData;
    public RenderPipelineAsset renderPipeline;
    public enum state
    {
        NONE,
        MIRROR,
        PUZZLE,
        END,
    }
    public state GameState;
    // Start is called before the first frame update
    void Start()
    {
        MirrorCanvasGroup.gameObject.SetActive(false);
        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Drag2.MirrorCrackNumber);
        switch (GameState)
        {
            case state.MIRROR:
                player.enabled = false;
                CanvasGroupTimer += Time.deltaTime;
                MirrorCanvasGroup.alpha = CanvasGroupTimer / 1;
                if (MirrorCanvasGroup.alpha >= 1)
                {
                    Camera.main.orthographic = true;
                    player.transform.position = MidPoint.position;
                    GameState = state.PUZZLE;
                }
                break;

            case state.PUZZLE:
                if (Drag2.MirrorCrackNumber == 13)
                {
                    mirrorTouch.playableDirector.Play();
                    Camera.main.orthographic = false;
                    GameState = state.END;
                }
                break;

            case state.END:
                player.enabled = true;
                break;
        }
    }
}
