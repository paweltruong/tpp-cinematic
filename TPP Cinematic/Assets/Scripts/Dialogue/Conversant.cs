using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds speaker settings in Conversation, and allows for Conversation to controll speaker animation
/// </summary>
[RequireComponent(typeof(AnimatorHelper))]
public class Conversant : MonoBehaviour
{
    [Tooltip("Text color on dialog UI")]
    [SerializeField] Color color;
    [SerializeField] ConversationSpeakerKey actor;
    [SerializeField] string displayName = "Unnamed";

    public ConversationSpeakerKey Speaker => actor;
    public string DisplayName => displayName;
    public Color Color => color;
    AnimatorHelper anim;

    private void Awake()
    {
        anim = GetComponent<AnimatorHelper>();
        if (anim == null)
            Debug.LogError($"{typeof(AnimatorHelper)} not found");
    }

    public void PlayAnimation(ConversationAnimation animation)
    {
        switch (animation)
        {
            case ConversationAnimation.ShakeHeadYes:
                anim.ShakeHeadYes();
                break;
            case ConversationAnimation.ShakeHeadNo:
                anim.ShakeHeadNo();
                break;
            case ConversationAnimation.PointBehind:
                anim.PointBehind();
                break;
            case ConversationAnimation.Yell:
                anim.Yell();
                break;
            default:                
                break;
        }
    }
}
