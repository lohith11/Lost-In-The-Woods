using UnityEngine;
using UnityEngine.AI;

public class MinionRoamState : MinionBaseState
{
    float _goToAttack = 0;
    public MinionRoamState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {
        minionStateManager.attackPlayer = false;
        Debug.Log("Entered roam state");
    }


    public override void UpdateState()
    {
        
        _goToAttack += Time.deltaTime;
        if (!minionStateManager.attackPlayer && _goToAttack > 15)
        {
            minionStateManager.switchState(minionStateManager.AttackState);
        }

        if (minionStateManager.minionAgent.remainingDistance <= minionStateManager.minionAgent.stoppingDistance) //* done with path
        {
            Vector3 point;
            if (RandomPoint(minionStateManager.centerPoint.transform.position, minionStateManager.sphereRadius, out point)) //* pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                minionStateManager.minionAgent.SetDestination(point);
            }
        }

        if(minionStateManager.PlayerInRange)
        {
            minionStateManager.switchState(minionStateManager.AttackState);
        }
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

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
        _goToAttack = 0;
    }

}
