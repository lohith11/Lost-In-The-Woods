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
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.Rotate(Vector3.up * playerStateMachine.playerRotation.x + Vector3.up * mouseX);
    }

    //public void CameraHeight()
    //{
    //    if(Mathf.Abs(transform.localPosition.y - desiredHeight) > 0.05f)
    //    {
    //        //transform.localPosition = Mathf.Lerp(currentPosition, playerStateMachine.standingHeight, 0.1f);
    //        playerStateMachine.playerCamera.localPosition = new Vector3(0, Mathf.Lerp(transform.localPosition.y, desiredHeight, 0.1f), 0.2f);
    //        //playerStateMachine.originalPosition = currentPosition;
    //    }
    //}
}
