using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f; // forward/backwards movement, sideways movement, up/down movement
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;

    // Animations for moving/rotation of ship
    public float lookRateSpeed = 90f;
    private Vector2 lookInput, screenCenter, mouseDistance; // Getting the mouse position
    /*
    The ship will turn based on where the mouse is moving (how far and what direction it is from the center of the screen. If the mouse is to the right of the screen, the ship will turn right)
    */

    private float rollInput; // Unity Project Settings Input manager              https://www.youtube.com/watch?v=J6QR4KzNeJU
    public float rollSpeed = 90f, rollAcceleration = 3.5f;


    // Start is called before the first frame update
    void Start()
    {
        // Find where the center of the screen is
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined; // Keep the mouse on the screen in-game
    }

    // Update is called once per frame
    void Update()
    {
        // Where the mouse is on the screen right now
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        
        mouseDistance.x = lookInput.x - screenCenter.x / screenCenter.y;
        mouseDistance.y = lookInput.y - screenCenter.y / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f); // No matter how much we move the mouse, we can't set the value of mouseDistance to be above 1 (it's clamped on 1 as a maximum value). TLDR consistent amount of rotation

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        // Apply the rotation on the ship
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self); // Rotate the ship itself, not the world // No rotations on z acis ('0f')

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position = transform.forward * activeForwardSpeed * Time.deltaTime; // .forward is whatever direction the ship is currently facing
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed) * Time.deltaTime; // Make the camera a child of the ship so the camera follows the ship
    }
}
