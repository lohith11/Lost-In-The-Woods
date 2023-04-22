using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimAssist : MonoBehaviour
{
   /* public float range = 10.0f;
    public float aimSpeed = 5.0f;
    public LayerMask enemyLayer;

    private Camera mainCamera;
    private bool isAiming = false;
    private RaycastHit hit;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        if (isAiming)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, range, enemyLayer))
            {
                Vector3 targetPosition = hit.collider.gameObject.transform.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);
            }
        }
    }*/
}
