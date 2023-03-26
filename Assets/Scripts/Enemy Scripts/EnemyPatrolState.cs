using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Walking_Anim");
        enemyStateManager.enemyAgent.speed = 1.5f;
        if(enemyStateManager.isWayPointPatrol)
        {
            enemyStateManager.nextLocation = enemyStateManager.waypoints[(enemyStateManager.destinationLoop++) % enemyStateManager.waypoints.Length].position;
        }
    }


    public override void UpdateState()
    {
        enemyStateManager.searchForSounds();

        if (enemyStateManager.isWayPointPatrol)
        {
            Vector3 directionToWalk = enemyStateManager.nextLocation - enemyStateManager.transform.position;
            Quaternion rotationToWayPoint = Quaternion.LookRotation(directionToWalk);
            enemyStateManager.transform.rotation = Quaternion.Slerp(enemyStateManager.transform.rotation, rotationToWayPoint , enemyStateManager.rotationSpeed * Time.deltaTime);
            if(Vector3.Distance(enemyStateManager.transform.position, enemyStateManager.nextLocation) < 1.0f && !enemyStateManager.PlayerInRange)
            {
                enemyStateManager.nextLocation = enemyStateManager.waypoints[(enemyStateManager.destinationLoop++ ) % enemyStateManager.waypoints.Length].position;
            }
            enemyStateManager.enemyAgent.SetDestination(enemyStateManager.nextLocation);
        }
        else
        {
            if (enemyStateManager.enemyAgent.remainingDistance <= enemyStateManager.enemyAgent.stoppingDistance) //* done with path
            {
                Vector3 point;
                if (RandomPoint(enemyStateManager.centerPoint.position, enemyStateManager.sphereRadius, out point)) //* pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                    enemyStateManager.enemyAgent.SetDestination(point);
                }
            }
        }

        if (enemyStateManager.PlayerInRange || enemyStateManager.SoundInRange)
        {
            enemyStateManager.switchState(enemyStateManager.AlertState);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //* Creates a sphere and assigns a random position to the vec3
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        //* Similar to Raycast hit
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public override void ExitState()
    {

    }
}
