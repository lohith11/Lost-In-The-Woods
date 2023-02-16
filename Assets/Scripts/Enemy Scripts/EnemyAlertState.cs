using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState (EnemyStateManager enemy):base(enemy){}
    private float _startChaseTimer = 2f;
    public override void EnterState()
    {
        
    }

    //start chase and notify enemies around you if the player is in range for more than 2 sec
    public override void UpdateState()
    {
        if(enemyStateManager.playerInRange)
        {
            _startChaseTimer -= Time.deltaTime;
        }
        if(_startChaseTimer <= 0)
        {
            enemyStateManager.switchState(enemyStateManager.chaseState);
            Debug.Log("Switching for chase state!");
        }

    
    }

    public override void ExitState()
    {
       
    }
}
