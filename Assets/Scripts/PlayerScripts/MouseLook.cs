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
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.Rotate(Vector3.up * mouseX);
    }

    //public IEnumerator CameraShakeWhileMoving(float duration, float magnitude)
    //{
    //    Vector3 originalPos = transform.localPosition;

    //    float elapsed = 0f;

    //    while(elapsed < duration)
    //    {
    //        float x = Random.Range(1f, 1f) * magnitude;
    //        float y = Random.Range(1f, 1f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, originalPos.z);

    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    transform.localPosition = originalPos;
    //}
}
