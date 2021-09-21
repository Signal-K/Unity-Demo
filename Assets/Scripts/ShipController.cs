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
    The ship will turn based on where the mouse is moving (how far and what direction it is from the center of the screen. If the mouse is to the right of the screen, the ship will turn right )
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position = transform.forward * activeForwardSpeed * Time.deltaTime; // .forward is whatever direction the ship is currently facing
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed) * Time.deltaTime; // Make the camera a child of the ship so the camera follows the ship
    }
}
