using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionRoamState : MinionBaseState
{
    float _goToAttack = 0;
    public MinionRoamState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {
        // minionStateManager.BackToAttack();
        minionStateManager.attackPlayer = false;
        Debug.Log("Entered roam state");
    }


    public override void UpdateState()
    {
        
        _goToAttack += Time.deltaTime;
      //  Debug.Log("Go to attack Timer is " + _goToAttack);
        if (!minionStateManager.attackPlayer && _goToAttack > 15)
        {
            minionStateManager.switchState(minionStateManager.AttackState);
        }
        // if (_goToAttack > 2)
        // {

        //     _goToAttack = 0;
        // }

        if (minionStateManager.minionAgent.remainingDistance <= minionStateManager.minionAgent.stoppingDistance) //* done with path
        {
            Vector3 point;
            if (RandomPoint(minionStateManager.playerRef.transform.position, minionStateManager.sphereRadius, out point)) //* pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //* so you can see with gizmos
                minionStateManager.minionAgent.SetDestination(point);
            }
        }
    }

    public override void ExitState()
    {
        /// minionStateManager.stopRoam();
        _goToAttack = 0;
       // Debug.Log("Exited Roam state");
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

}
