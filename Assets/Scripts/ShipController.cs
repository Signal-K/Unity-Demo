using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f; // forward/backwards movement, sideways movement, up/down movement
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeForwardSpeed = Input.GetAxisRaw("Vertical") * forwardSpeed;
        activeStrafeSpeed = Input.GetAxisRaw("Horizontal") * strafeSpeed;
        activeHoverSpeed = Input.GetAxisRaw("Hover") * hoverSpeed; // Created hover axis in Unity project settings

        transform.position = transform.forward * activeForwardSpeed * Time.deltaTime; // .forward is whatever direction the ship is currently facing
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed) * Time.deltaTime; // Make the camera a child of the ship so the camera follows the ship
    }
}
