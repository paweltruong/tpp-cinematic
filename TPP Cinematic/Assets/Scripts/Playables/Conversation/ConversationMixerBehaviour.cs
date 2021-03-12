using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// Controls Conversation workflow on a timeline on clip bounds
/// </summary>
public class ConversationMixerBehaviour : PlayableBehaviour
{
    Conversation boundConversation;
    ConversationSpeakerKey lastSpeaker = ConversationSpeakerKey.Unknown;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        boundConversation = playerData as Conversation;

        if (boundConversation == null)
            return;

        int inputCount = playable.GetInputCount();
        int currentInputs = 0;
        float activeWeight = 0; ;
        int activeInputIndex = -1;
        ConversationSpeakerKey activeSpeaker = lastSpeaker;

        //Set active speaker
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ConversationBehaviour> inputPlayable = (ScriptPlayable<ConversationBehaviour>)playable.GetInput(i);
            ConversationBehaviour input = inputPlayable.GetBehaviour();

            if (!Mathf.Approximately(inputWeight, 0f))
            {
                currentInputs++;
                if (activeWeight <= inputWeight)
                {
                    activeWeight = inputWeight;
                    activeInputIndex = i;
                    activeSpeaker = input.speaker;
                }
            }
        }

        //Control Conversation Workflow
        if (inputCount > 0)
        {
            if (lastSpeaker == ConversationSpeakerKey.Unknown && activeSpeaker != ConversationSpeakerKey.Unknown)
            {
                boundConversation.DisplayCurrent();
            }
            else if (lastSpeaker != activeSpeaker)
            {
                boundConversation.SetAndDisplayNextNode();
            }
            else if (activeSpeaker == ConversationSpeakerKey.Unknown)
            {
                boundConversation.DisplayDefault();
            }
            lastSpeaker = activeSpeaker;
        }
    }
}
