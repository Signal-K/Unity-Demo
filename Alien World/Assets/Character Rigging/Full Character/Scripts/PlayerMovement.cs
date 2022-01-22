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

    [SerializeField] private float jumpHeight;

    // References
    private CharacterController controller;
    private Animator anim;

    private void Start() {
        // Gets called whenever the game starts
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); // Set the animator to be equal to the player animator (into first child of the "Player" game object)
    }

    private void Update() {
        // Called each frame
        Move();

        if (Input.GetKeyDown(KeyCode.Mouse0)) { // If we press left click
            StartCoroutine(Attack());
        }
    }

    private void Move() {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); // Return true whenever we're standing on the ground

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical"); // z = forward and backwards input axes

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection); // The forward is tied to the direction the player is facing in

        if(isGrounded) { // If the player is on the ground, we can walk/move the player
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

            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime; // calculate gravity
        controller.Move(velocity * Time.deltaTime); // apply gravity to the character
    }

    private void Idle() {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk() {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run() {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime); // Params -> float, value, dampTime value, T.dT value
    }

    private void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); // Amount the player will jump (move upwards on the y axis)
    }

    private IEnumerator Attack() {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack layer"), 1); // Set the weight of the attack layer to 1
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack layer"), 0); // Set the weight of the attack layer to 0
    }
}
