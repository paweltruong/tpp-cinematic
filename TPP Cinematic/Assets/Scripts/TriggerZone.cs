using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// fires events when trigger with object on specified layer occurs
/// </summary>
public class TriggerZone : MonoBehaviour
{
    [SerializeField] LayerMask collisionFilter;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (collisionFilter.value == (collisionFilter.value | 1 << other.gameObject.layer))
            onTriggerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisionFilter.value == (collisionFilter.value | 1 << other.gameObject.layer))
            onTriggerExit?.Invoke();
    }
}
