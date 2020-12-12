using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class Play02 : PlayableAsset
{
    public ExposedReference<ParticleSystem> RingEffect2;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        Play RingEffectPlayable = new Play();

        RingEffectPlayable.RingEffect = RingEffect2.Resolve(graph.GetResolver());

        RingEffectPlayable.RingEffect.Play();

        return ScriptPlayable<Play>.Create(graph, RingEffectPlayable);
    }
}
