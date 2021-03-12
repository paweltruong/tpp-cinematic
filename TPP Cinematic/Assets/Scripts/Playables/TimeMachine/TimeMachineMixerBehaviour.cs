using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//TODO: fix intant option selection
//TODO: merge with conversationTrack and behaviour?
/// <summary>
/// Handles looping and jumping based on Conversation state
/// </summary>
public class TimeMachineMixerBehaviour : PlayableBehaviour
{
    public Dictionary<ConversationSpeakerKey, double> markerClips;
    private PlayableDirector director;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!Application.isPlaying)
            return;

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<TimeMachineBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            if (inputWeight > 0f)
            {
                if (!input.clipExecuted)
                {
                    switch (input.action)
                    {
                        case TimeMachineBehaviour.TimeMachineAction.JumpToMarker:
                            if (input.conversation.IsLeafNode())
                            {
                                //end conversation
                            }
                            else
                            {
                                //TODO: fix intant option selection
                                ConversationSpeakerKey markerToJumpTo = ConversationSpeakerKey.Unknown;
                                if (input.conversation.IsWaitingForSelection)
                                {
                                    //still waiting for option
                                    if (input.conversation.Current != null)
                                        markerToJumpTo = input.conversation.Current.Speaker;
                                    else
                                        Debug.LogError("Current node not found");
                                }
                                else if (!string.IsNullOrEmpty(input.conversation.SelectedOption))
                                {
                                    // jump to selected option
                                    markerToJumpTo = input.conversation.GetNextNodeForOption().Speaker;
                                }
                                else if (input.conversation.IsNextNodeSameSpeaker())
                                {
                                    //jump to speaker marker
                                    if (input.conversation.Current != null)
                                        markerToJumpTo = input.conversation.Current.Speaker;
                                    else
                                        Debug.LogError("Current node not found");
                                }
                                else if (input.conversation.IsNextNodeDifferentSpeaker())
                                {
                                    // jump to other marker
                                    markerToJumpTo = input.conversation.GetNextNode().Speaker;
                                }

                                double t = markerClips[markerToJumpTo];
                                (playable.GetGraph().GetResolver() as PlayableDirector).time = t;
                                input.clipExecuted = false; //we want the jump to happen again!
                            }
                            break;
                    }
                }
            }
        }
    }
}
