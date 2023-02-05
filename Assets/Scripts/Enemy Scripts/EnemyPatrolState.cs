using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Enemy.enemyAnimController.SetBool("Patrol", true);
        Enemy.EnemyAgent.speed = 1.5f;
    }


    public override void UpdateState(EnemyStateManager Enemy)
    {

        if (Enemy.EnemyAgent.remainingDistance <= Enemy.EnemyAgent.stoppingDistance) //* done with path
        {
            Debug.Log("Patroooooooooling"); //!
            Vector3 point;
            if (RandomPoint(Enemy.centrePoint.position, Enemy.sphereRaidus, out point, Enemy)) //* pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                Enemy.EnemyAgent.SetDestination(point);
            }
        }
        else if (Enemy.playerInRange)
        {
            Enemy.switchState(Enemy.alertState);
            Debug.Log("Switching the state to alert"); //!
        }
    }



    bool RandomPoint(Vector3 center, float range, out Vector3 result, EnemyStateManager Enemy)
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

    public override void ExitState(EnemyStateManager Enemy)
    {
        
    }
}
