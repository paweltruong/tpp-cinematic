using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ConversationClip : PlayableAsset, ITimelineClipAsset
{
    public ConversationBehaviour template = new ConversationBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ConversationBehaviour>.Create(graph, template);
        return playable;
    }
}
