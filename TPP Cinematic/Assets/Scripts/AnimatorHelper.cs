using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class to ease switching and playing animations
/// </summary>
public class AnimatorHelper : MonoBehaviour
{
    [Tooltip("To add variation in blinking timing for different characters")]
    [Range(0,1f)]
    [SerializeField] float blinkingOffset = 0f;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat(Constants.AnimationParameters.Par_BlinkingOffset, blinkingOffset);
    }

    public void StartWalk(bool forward)
    {
        anim.SetBool(Constants.AnimationParameters.Par_IsWalking, true);
        anim.SetBool(Constants.AnimationParameters.Par_IsForward, forward);
    }
    public void StopWalk()
    {
        anim.SetBool(Constants.AnimationParameters.Par_IsWalking, false);
    }

    public void Jab()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_Jab);
    }

    public void ReceivedKillingBlow()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_HeadHit);
        anim.SetBool(Constants.AnimationParameters.Par_Dead, true);
    }

    public void Bow()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_FormalBow);
    }

    public void ShakeHeadNo()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_ShakeHeadNo);
    }
    public void ShakeHeadYes()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_ShakeHeadYes);
    }
    public void PointBehind()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_PointBehind);
    }

    public void Yell()
    {
        anim.SetTrigger(Constants.AnimationParameters.Trig_Yell);
    }

    /// <summary>
    /// stay surprise doe X seconds
    /// </summary>
    /// <param name="duration"></param>
    public void Surprised(float duration)
    {
        anim.SetBool(Constants.AnimationParameters.Par_IsSurprised, true);
        Invoke(nameof(StopSurprised), duration);
    }

    void StopSurprised()
    {
        anim.SetBool(Constants.AnimationParameters.Par_IsSurprised, false);
    }


}
