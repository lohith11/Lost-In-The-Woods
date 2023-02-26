using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.SetBool("Patrol", true);
        enemyStateManager.EnemyAgent.speed = 1.5f;
    }


    public override void UpdateState()
    {
        if (enemyStateManager.EnemyAgent.remainingDistance <= enemyStateManager.EnemyAgent.stoppingDistance) //* done with path
        {
            Vector3 point;
            if (RandomPoint(enemyStateManager.centerPoint.position, enemyStateManager.sphereRaidus, out point)) //* pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                enemyStateManager.EnemyAgent.SetDestination(point);
            }
        }
        else if (enemyStateManager.playerInRange)
        {
            enemyStateManager.switchState(enemyStateManager.AlertState);
            
        }
    } 

    private IEnumerator CheckForSounds()
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapSphere(enemyStateManager.transform.position, enemyStateManager.hearingRange);
            foreach(Collider collider in colliders)
            {
                MakeSound sound = collider.GetComponent<MakeSound>();
                if(sound != null)
                {
                    float distance = Vector3.Distance(enemyStateManager.transform.position, collider.transform.position);
                    if(distance < sound.soundRange)
                    {
                        //* Make the enemy move to the audio location and switch to alert state
                    }
                }
            }
            yield return new WaitForSeconds(enemyStateManager.soundCheckInterval);
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
        enemyStateManager.enemyAnimController.SetBool("Patrol", false);
    }
}
