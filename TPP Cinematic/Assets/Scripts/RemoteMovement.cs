using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoteMovement : MonoBehaviour
{
    [SerializeField] float minDestinationDistance = .5f;
    Vector3 destination;

    NavMeshAgent agent;
    CharacterController controller;
    AnimatorHelper anim;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AnimatorHelper>();
        if(agent.enabled)
            agent.isStopped = true;
    }
    
    void Update()
    {
        if(agent.enabled && !agent.isStopped && agent.remainingDistance <= minDestinationDistance )
        {
            agent.isStopped = true;
            anim.StopWalk();
        }
    }

    public void MoveToPoint(Transform destination)
    {
        this.destination = destination.position;
        agent.SetDestination(destination.position);
        agent.isStopped = false;
        anim.StartWalk(true);
    }

    public void ChangeAgentOffset(float offset)
    {
        agent.baseOffset = offset;
    }
}
