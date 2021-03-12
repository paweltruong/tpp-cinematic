using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float gravity = 9.81f;
    public bool isControlledByPlayer;
    
    Rigidbody rb;
    CharacterController controller;
    AnimatorHelper animHelper;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animHelper = GetComponent<AnimatorHelper>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isControlledByPlayer)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            Walk(vertical);
            Rotate(horizontal);

            ApplyGravity();

            if (Input.GetKeyDown(KeyCode.E))
            {
                animHelper.Jab();
            }           
        }
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
            controller.Move(-transform.up * gravity * Time.deltaTime);
    }

    void Walk(float verticalInput)
    {
        if (verticalInput != 0)
        {
            var forward = verticalInput > 0;
            var localDirection = forward ? transform.forward : -transform.forward;
            controller.Move(localDirection * speed * Time.deltaTime);
            animHelper.StartWalk(forward);
        }
        else
        {
            animHelper.StopWalk();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotationImpulse"> 1 for right turn -1 for left turn</param>
    void Rotate(float rotationImpulse)
    {
        if (rotationImpulse != 0)
        {
            transform.Rotate(0f, rotationImpulse * rotationSpeed, 0f);
        }
    }

    public void TogglePlayerControl(bool playerControllEnabled)
    {
        isControlledByPlayer = playerControllEnabled;
    }
}
