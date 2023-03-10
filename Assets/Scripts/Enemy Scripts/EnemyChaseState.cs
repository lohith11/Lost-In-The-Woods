using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{

    //todo : if the player does not in line of sight for 2 sec switch back to alert state 

    //* if attack -> attack function -> spear IK -> spear do damange event -> player health sub to event
    public EnemyChaseState(EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Chasing Anim");
        Debug.Log("Entered the chase state"); //! delete the debug
    }


    public override void UpdateState()
    {   
        float distanceToPlayer = Vector3.Distance(enemyStateManager.transform.position , enemyStateManager.playerRef.transform.position);
        if(distanceToPlayer <= enemyStateManager.attackRadius)
        {
            Debug.Log("The player is in the attack raneg");
            AttackPlayer();
        }
        if(distanceToPlayer > enemyStateManager.attackRadius && enemyStateManager.PlayerInRange)
        {
            Debug.Log("The player is in chase range");
            ChasePlayer();
        }

        if(!enemyStateManager.PlayerInRange)
        {
            enemyStateManager.timeSinceLastSighting += Time.deltaTime;
        }
        if(enemyStateManager.timeSinceLastSighting >= 2f)
        {
            enemyStateManager.switchState(enemyStateManager.AlertState);
            Debug.Log("Player out of range switching back to the alert state"); //!
        }

    }

    void AttackPlayer()
    {
        //enemyStateManager.EnemyAgent.velocity = Vector3.zero;
        enemyStateManager.EnemyAgent.stoppingDistance = 2f;
        Debug.Log("Attack function called");
        //* play attack anim
        //* deal damage to the player
    }

    void ChasePlayer()
    {
        Debug.Log("Chase function called");
        Vector3 directionToPlayer = enemyStateManager.playerRef.transform.position - enemyStateManager.transform.position;
        float distanceToMove = Mathf.Max(directionToPlayer.magnitude - enemyStateManager.minDistanceToPlayer , 0f);
        Vector3 targetPosition = enemyStateManager.transform.position + directionToPlayer.normalized * distanceToMove;
        enemyStateManager.EnemyAgent.SetDestination(targetPosition);

        if(!enemyStateManager.isAttacking)
        {
            AttackPlayer();
        }
    }
    
    public override void ExitState()
    {
       enemyStateManager.timeSinceLastSighting = 0f;
    }

}
