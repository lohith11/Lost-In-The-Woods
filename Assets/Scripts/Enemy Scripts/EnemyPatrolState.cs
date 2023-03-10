using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Walking Anim");
        enemyStateManager.EnemyAgent.speed = 1.5f;
    }


    public override void UpdateState()
    {
        enemyStateManager.searchForSounds();

        if (enemyStateManager.EnemyAgent.remainingDistance <= enemyStateManager.EnemyAgent.stoppingDistance) //* done with path
        {
            Vector3 point;
            if (RandomPoint(enemyStateManager.centerPoint.position, enemyStateManager.sphereRadius, out point)) //* pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                enemyStateManager.EnemyAgent.SetDestination(point);
            }
        }
        else if(enemyStateManager.PlayerInRange || enemyStateManager.SoundInRange)
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
