using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GameControllerLv1 : MonoBehaviour
{
    public CanvasGroup MirrorCanvasGroup;
    public Player player;
    float CanvasGroupTimer;

    public UniversalRenderPipelineAsset cameraData;
    public RenderPipelineAsset renderPipeline;
    public enum state
    {
        NONE,
        MIRROR,
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

        switch (GameState)
        {
            case state.MIRROR:
                player.enabled = false;
                CanvasGroupTimer += Time.deltaTime;
                MirrorCanvasGroup.alpha = CanvasGroupTimer / 1;
                if (MirrorCanvasGroup.alpha >= 1)
                {
                    Camera.main.orthographic = true;
                }
                break;
        }
    }

    public void MirrorCanOpen()
    {
        if (player.isObstacle == true)
        {
            MirrorCanvasGroup.gameObject.SetActive(true);
            GameState = state.MIRROR;
        }
    }
}
