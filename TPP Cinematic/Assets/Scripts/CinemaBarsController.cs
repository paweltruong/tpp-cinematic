using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adds/removes cinema bar effect on the screen (with fade)
/// </summary>
public class CinemaBarsController : MonoBehaviour
{
    [SerializeField] Image topBar;
    [SerializeField] Image bottomBar;
    [SerializeField] float fadeDuration;
    [SerializeField] float targetHeight;
    float tempTargetHeight;
    float changeSize;
    bool fadeCompleted;

    void Update()
    {
        if (!fadeCompleted)
        {
            var sizeDelta = topBar.rectTransform.sizeDelta;
            sizeDelta.y += changeSize * Time.deltaTime;
            if ((changeSize > 0 && sizeDelta.y > tempTargetHeight)
                || (changeSize < 0 && sizeDelta.y < tempTargetHeight))
            {
                fadeCompleted = true;
            }
            topBar.rectTransform.sizeDelta = sizeDelta;
            bottomBar.rectTransform.sizeDelta = sizeDelta;

        }
    }

    public void Show()
    {
        tempTargetHeight = targetHeight;
        changeSize = (tempTargetHeight - topBar.rectTransform.sizeDelta.y) / fadeDuration;
        fadeCompleted = false;
    }
    public void Hide()
    {
        tempTargetHeight = 0f;
        changeSize = (tempTargetHeight - topBar.rectTransform.sizeDelta.y) / fadeDuration;
        fadeCompleted = false;
    }
}
