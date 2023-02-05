using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public static LineOfSight losInstance; //* Singleton
    public event EventHandler OnPlayerFound;
    public event EventHandler OnlostPlayer;
    public float viewRadius;

    //* [Header("test")]
    //* [Space (10)]
    [Range(0,360)]
    public float viewAngle;
    public LayerMask PlayerMask;
    public LayerMask obstacleMask;
    public List<Transform> visbleTargets = new List<Transform>();

    private void Awake() 
    {
        losInstance = this;
    }

    private void Update() 
    {
        StartCoroutine(FindVisbleTargets());
    }

    IEnumerator FindVisbleTargets()
    {
        visbleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position , viewRadius , PlayerMask); //* collects all the collider data that is in the radius

        for(int i = 0; i<targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized; //* calculates the direction to the targte
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2) //*checks if the target is the FOV or not
            {
                float disToTarget = Vector3.Distance(transform.position , target.position); //* calculates the distance to the target

                if(!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask)) //* checks if we have direct line of sight with the player or not
                {
                    Debug.Log("Found player!"); //! delete this later
                    OnPlayerFound?.Invoke(this,EventArgs.Empty);
                    visbleTargets.Add(target);
                }
            }
            else
            {
                OnlostPlayer?.Invoke(this,EventArgs.Empty);
            }
        }
        yield return new WaitForSeconds(5f);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y; //* converts the local angle to glocal angle
        }

        //* When we use an angle we swap between sin and cos as unity angle system starts clockwise and trigonmetry starts anti clock wise
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
