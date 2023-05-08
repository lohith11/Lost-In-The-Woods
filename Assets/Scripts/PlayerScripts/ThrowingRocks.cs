using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowingRocks : MonoBehaviour
{
    public GameObject lineRendererEndPoint;

    [Header("References")]
    public Transform cam;
    public Transform attackpoint;
    public GameObject objectThrow;
    public GameObject pickingThings;
    public TMP_Text rockCount;
    public GameObject potIsthere;
    public GameObject playerHud;

    public int maxRockPickUp;
    [ShowInInspector] public static int totalThrows;
    public float throwCoolDown;

    public float throwForce;
    public float throwUpwardForce;
    private LineRenderer lineRenderer;

    private bool readyToThrow;
    public bool canPickUp;

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
    private GameObject potInRange;
    //The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;

    [Space(5)]
    [Header("< PotVariables >")]
    public bool isOilPot = false;
    public GameObject oilPot;
    public bool canPickPot;
    public int totalPots;

    void Start()
    {
        potIsthere.SetActive(false);
        pickingThings.SetActive(false);
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
            playerHud.SetActive(true);
            if (playerStateMachine.isAtttacking && readyToThrow && totalThrows > 0)
            {
                if (!isOilPot)
                {
                    Throw();
                }
            }
            if (playerStateMachine.isAtttacking && isOilPot && totalPots > 0 && readyToThrow)
            {
                PotThrow();
            }

            Projectile();
        }

        else
        {
            lineRenderer.enabled = false;
            playerHud.SetActive(false);
            lineRendererEndPoint.SetActive(false);
        }

        if (totalPots >= 1)
        {
            canPickPot = false;
            totalPots = 1;
        }

        if (totalThrows >= maxRockPickUp)
        {
            canPickUp = false;
            totalThrows = maxRockPickUp;
        }
    }

    public void PotThrow()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(oilPot, attackpoint.position, cam.rotation);

        //Getting RigidBody of that throwable object
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.useGravity = true;

        SphereCollider projectileCollider = projectile.GetComponent<SphereCollider>();
        projectileCollider.isTrigger = false;

        //Caluculate Direction of that Object
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackpoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalPots--;
        potIsthere.SetActive(false);
        Invoke(nameof(ResetThrow), throwCoolDown);
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
            if (hit.collider.gameObject.layer == enemyLayer) // if aim is locked on enemy
            {
                forceDirection = (hit.collider.gameObject.transform.position - attackpoint.position).normalized;
            }
            else // if aim is not locked on enemy
            {
                forceDirection = cam.transform.forward;
            }
        }

        Vector3 forceToAdd = forceDirection * throwForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        rockCount.text = totalThrows.ToString();
        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void Projectile()
    {

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, distance, enemyLayer))
        {
            // If aiming at an enemy, hide the line renderer
            lineRenderer.enabled = false;
            lineRendererEndPoint.SetActive(false);
            crossHair.SetActive(true);

            Vector3 targetPosition = hit.collider.gameObject.transform.position;
            targetPosition.y = transform.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, aimSpeed);
        }
        else
        {
            // If not aiming at an enemy, show the line renderer
            lineRenderer.enabled = true;
            lineRendererEndPoint.SetActive(true);
            crossHair.SetActive(false);
            lineRenderer.positionCount = numPoints;

            List<Vector3> points = new List<Vector3>();
            Vector3 startingPosition = attackpoint.position;
            Vector3 startingVelocity = cam.transform.forward * throwForce;
            for (float t = 0; t < numPoints; t += timeBetweenPoints)
            {
                newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / f * t * t;

                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 0.01f, CollidableLayers).Length > 0)
                {
                    lineRenderer.positionCount = points.Count;
                    Vector3 target = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                    lineRendererEndPoint.transform.localPosition = target;
                    break;
                }
            }

            lineRenderer.SetPositions(points.ToArray());
        }
    }

    public void RockPicking()
    {
        if (canPickUp)
        {
            playerStateMachine.audioSource.PlayOneShot(rockPickingSound);
            totalThrows++;
            rockCount.text = totalThrows.ToString();
            pickingThings.SetActive(false);
            Destroy(rockInRange);
            rockInRange = null;
            canPickUp = false;
        }
    }

    public void PotPicking()
    {
        if (canPickPot)
        {
            totalPots++;
            isOilPot = true;
            potIsthere.SetActive(true);
            pickingThings.SetActive(false);
            Destroy(potInRange);
            potInRange = null;
            canPickPot = false;
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            pickingThings.SetActive(true);
            canPickUp = true;
            playerStateMachine.playerControls.Player.Picking.performed += playerStateMachine.PickingRock;
            rockInRange = other.gameObject;
        }

        if (other.gameObject.CompareTag("OilPot"))
        {
            pickingThings.SetActive(true);
            canPickPot = true;
            playerStateMachine.playerControls.Player.Picking.performed += playerStateMachine.PickingPot;
            potInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            pickingThings.SetActive(false);
            canPickUp = false;
            playerStateMachine.playerControls.Player.Picking.performed -= playerStateMachine.PickingRock;
            rockInRange = null;
        }

        if (other.gameObject.CompareTag("OilPot"))
        {
            pickingThings.SetActive(false);
            canPickPot = false;
            playerStateMachine.playerControls.Player.Picking.performed -= playerStateMachine.PickingPot;
            potInRange = null;
        }
    }
    #endregion

    public void ResetThrow()
    {
        readyToThrow = true;
        isOilPot = false;
    }
}