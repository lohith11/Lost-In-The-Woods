using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed;
    private float xRotation = 0f;
    public Transform playerObject;
    private PlayerStateMachine playerStateMachine;

    private void Start()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Rotation();
    }

    public void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.Rotate(Vector3.up * playerStateMachine.playerRotation.x + Vector3.up * mouseX);
    }
}
