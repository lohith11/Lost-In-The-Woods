using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed;
    private float xRotation = 0f;
    public Transform playerObject;
    //public float desiredHeight;
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
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Vector3 rot = Vector3.up * mouseX;
        playerObject.Rotate(rot);
    }
}
