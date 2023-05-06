using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Microsoft.Unity.VisualStudio.Editor;

public class ThrowingRocks : MonoBehaviour
{
    public GameObject lineRendererEndPoint;

    [Header("References")]
    public Transform cam;
    public Transform attackpoint;
    public GameObject objectThrow;
    public TMP_Text pressRocksText;

    public int maxRockPickUp;
    public static int totalThrows;
    public float throwCoolDown;

    public float throwForce;
    public float throwUpwardForce;
    private LineRenderer lineRenderer;

    private bool readyToThrow;
    public bool canPickUp = false;

    public float f;
    public int numPoints = 50;
    public float timeBetweenPoints;
    private PlayerStateMachine playerStateMachine;

    public float distance;
    public LayerMask enemyLayer;
    private RaycastHit hit;
    public float aimSpeed;

    public GameObject crossHair;
    private Vector3 newPoint;
    public AudioClip rockPickingSound;
    private GameObject rockInRange;
    //The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        crossHair.SetActive(false);
        lineRendererEndPoint.SetActive(false);
        readyToThrow = true;
        playerStateMachine = GetComponent<PlayerStateMachine>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (playerStateMachine.isAiming)
        {
            if (playerStateMachine.isAtttacking && readyToThrow && totalThrows > 0)
            {
                Throw();
            }

            Projectile();
        }

        else
        {
            lineRenderer.enabled = false;
            crossHair.SetActive(false);
            lineRendererEndPoint.SetActive(false);
        }

        if(totalThrows >= maxRockPickUp)
        {
            canPickUp = false;
            totalThrows = maxRockPickUp;
        }
        else if(totalThrows < maxRockPickUp)
        {
            canPickUp = true;
        }
    }

    public void Throw()
    {
        readyToThrow = false;
        
        //Instatiation object
        GameObject projectile = Instantiate(objectThrow, attackpoint.position, cam.rotation);

        projectile.GetComponent<RockDestroy>().isThrown = true;

        //Getting RigidBody of that throwable object
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        //Caluculate Direction of that Object
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackpoint.position).normalized;
        }

        //Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        Vector3 forceToAdd = forceDirection * throwForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void Projectile()
    {
        crossHair.SetActive(true);
        lineRendererEndPoint.SetActive(true);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, distance, enemyLayer))
        {
            Vector3 targetPosition = hit.collider.gameObject.transform.position;
            targetPosition.y = transform.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
            lineRendererEndPoint.transform.LookAt(targetPosition);
        }

        lineRenderer.enabled = true;
        lineRenderer.positionCount = numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = attackpoint.position;
        Vector3 startingVelocity = cam.transform.forward * throwForce;
        for (float t = 0; t < numPoints; t += timeBetweenPoints)
        {
            newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / f * t * t;

            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 0.05f, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                Vector3 target = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                lineRendererEndPoint.transform.position = target;
                break;
            }
        }

        lineRenderer.SetPositions(points.ToArray());
    }

    public void RockPicking()
    {
        playerStateMachine.audioSource.PlayOneShot(rockPickingSound);
        if(canPickUp)
        {
            totalThrows++;
            pressRocksText.enabled = false;
            Destroy(rockInRange);
            rockInRange = null;
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rock"))
        {
            pressRocksText.enabled = true;
            canPickUp = true;
            playerStateMachine.playerControls.Player.Picking.performed += playerStateMachine.PickingRock;
            pressRocksText.text = "Press E or Controller Y";
            rockInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            pressRocksText.enabled = false;
            canPickUp = false;
            playerStateMachine.playerControls.Player.Picking.performed -= playerStateMachine.PickingRock;
            rockInRange = null;
        }
    }
    #endregion

    public void ResetThrow()
    {
        readyToThrow = true;
    }
}