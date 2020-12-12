using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class FrontSortLayer02 : PlayableAsset
{
    public ExposedReference<SpriteRenderer> sprite2;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        SortLayer spritePlayable = new SortLayer();

        spritePlayable.sprite = sprite2.Resolve(graph.GetResolver());

        spritePlayable.sprite.sortingLayerName = "Front";

        return ScriptPlayable<SortLayer>.Create(graph, spritePlayable);
    }
}
