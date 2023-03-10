using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Alert Anim");
    }

    //start chase and notify enemies around you if the player is in range for more than 2 sec
    public override void UpdateState()
    {
        if(enemyStateManager.PlayerInRange)
        {
            enemyStateManager.startChaseTimer -= Time.deltaTime;
        }
        if(enemyStateManager.startChaseTimer <= 0)
        {
            enemyStateManager.switchState(enemyStateManager.ChaseState);
            Debug.Log("Switching for chase state!"); //!
        }

        if(enemyStateManager.SoundInRange)
        {
            enemyStateManager.transform.position = enemyStateManager.targetPosition; //! should make the enemy stop 0.5f before the target
        }

    
    }

    public override void ExitState()
    {
       enemyStateManager.startChaseTimer = 3.0f;
    }
}
