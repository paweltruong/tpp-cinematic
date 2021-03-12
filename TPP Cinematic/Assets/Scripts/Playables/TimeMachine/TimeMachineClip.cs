using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineClip : PlayableAsset, ITimelineClipAsset
{
	[HideInInspector]
    public TimeMachineBehaviour template = new TimeMachineBehaviour ();
	public TimeMachineBehaviour.TimeMachineAction action;
    public ConversationSpeakerKey markerLabel;


    public ExposedReference<Conversation> conversation;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TimeMachineBehaviour>.Create (graph, template);
        TimeMachineBehaviour clone = playable.GetBehaviour ();        
		clone.action = action;
		clone.markerLabel = markerLabel;
        clone.conversation = conversation.Resolve(graph.GetResolver());

        return playable;
    }
}
