using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed;
    private float xRotation = 0f;
    public Transform playerObject;
    private float originalPosition = 0f;
    private float timer;
    private PlayerStateMachine stateMachine;

    private void Start()
    {
        stateMachine = FindObjectOfType<PlayerStateMachine>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        originalPosition = transform.localPosition.y;
        Rotation();
    }

    public void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.Rotate(Vector3.up * mouseX);
    }
}
