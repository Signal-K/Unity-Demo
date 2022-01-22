using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Variables
    [SerializeField] private float mouseSensitivity;

    // References
    private Transform parent;

    private void Start() {
        parent = transform.parent;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        Rotate();
    }

    private void Rotate() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Value of moving the mouse left and right

        parent.Rotate(Vector3.up, mouseX);
    }
}
