using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    
    private Vector3 moveDirection; // direction we're moving in
    private Vector3 velocity;

    [SerializeField] private bool isGrounded; // Is the player on the ground/surface?
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    // References
    private CharacterController controller;

    private void Start() {
        // Gets called whenever the game starts
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        // Called each frame
        Move();
    }

    private void Move() {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); // Return true whenever we're standing on the ground

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical"); // z = forward and backwards input axes

        moveDirection = new Vector3(0, 0, moveZ);
        
        if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) { // if not (0,0,0) and the left shift key is NOT being held down
            // We are walking
            Walk();
        }
        else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) {
            // We are sprinting/running
            Run();
        }
        else if(moveDirection == Vector3.zero) { // If there is no movement
            // We are idle/not moving
            Idle();
        }

        moveDirection *= moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime; // calculate gravity
        controller.Move(velocity * Time.deltaTime); // apply gravity to the character
    }

    private void Idle() {
        
    }

    private void Walk() {
        moveSpeed = walkSpeed;
    }

    private void Run() {
        moveSpeed = runSpeed;
    }
}
