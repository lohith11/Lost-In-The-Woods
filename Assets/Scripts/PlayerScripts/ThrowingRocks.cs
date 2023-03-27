using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingRocks : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackpoint;
    public GameObject objectThrow;

    public int totalThrows;
    public float throwCoolDown;

    public float throwForce;
    public float throwUpwardForce;
    private LineRenderer lineRenderer;

    private bool readyToThrow;

    public float f;
    public int numPoints = 50;
    public float timeBetweenPoints;
    private PlayerStateMachine playerStateMachine;

    //The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        lineRenderer = GetComponent<LineRenderer>();
        readyToThrow = true;
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
        }
    }

    public void Throw()
    {
        readyToThrow = false;

        //Instatiation object
        GameObject projectile = Instantiate(objectThrow, attackpoint.position, cam.rotation);

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
        Vector3 forceToAdd=forceDirection * throwForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void Projectile()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = (int)numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = attackpoint.position;
        Vector3 startingVelocity = cam.transform.forward * throwForce;
        for (float t = 0; t < numPoints; t += timeBetweenPoints)
        {
            Vector3 newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / f * t * t;
            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
        }

        lineRenderer.SetPositions(points.ToArray());
    }

    public void PickingRock()
    {

    }

    public void ResetThrow()
    {
        readyToThrow = true;
    }
}

