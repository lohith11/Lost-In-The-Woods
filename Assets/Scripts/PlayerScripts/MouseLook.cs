using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed;
    public float controllerSpeed;
    private float xRotation = 0f;
    public Transform playerObject;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Rotation(PlayerInput playerInput, Vector2 rotation)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard": rotation *= (mouseSpeed * Time.deltaTime); break;
            case "Controller": rotation *= (controllerSpeed * Time.deltaTime); break;
            default: Debug.LogError($"Control Scheme {playerInput.currentControlScheme} not expected!"); break;
        }

        xRotation -= rotation.y;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.Rotate(Vector3.up * rotation.x);
    }
}
