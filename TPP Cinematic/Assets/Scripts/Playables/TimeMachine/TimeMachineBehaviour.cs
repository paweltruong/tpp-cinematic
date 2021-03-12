using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineBehaviour : PlayableBehaviour
{
	public TimeMachineAction action;
    public ConversationSpeakerKey markerLabel;
    public Conversation conversation;

	[HideInInspector]
	public bool clipExecuted = false; //the user shouldn't author this, the Mixer does

	public enum TimeMachineAction
	{
		Marker,
		JumpToMarker,
	}
}
