using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    float _alertSpeed = 2.0f;
    public EnemyAlertState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Alert_Anim");
        //! to make it better add a exclmation point and a small bar on top of the enemy 
    }

    public override void UpdateState()
    {
        if(enemyStateManager.PlayerInRange)
        {
            enemyStateManager.startChaseTimer -= Time.deltaTime;
            enemyStateManager.backToPatrol = 2.0f;
        }
        if(enemyStateManager.startChaseTimer <= 0)
        {
            enemyStateManager.switchState(enemyStateManager.ChaseState);
            Debug.Log("Switching for chase state!"); //!
        }

        if(enemyStateManager.SoundInRange)
        {
            enemyStateManager.enemyAgent.SetDestination(enemyStateManager.soundPosition);
            enemyStateManager.enemyAnimController.Play("Finding_Anim");
        }

        if(!enemyStateManager.PlayerInRange)
            enemyStateManager.backToPatrol -= Time.deltaTime;
        if(enemyStateManager.backToPatrol <= 0)
            enemyStateManager.switchState(enemyStateManager.PatrolState);
        

    
    }

    public override void ExitState()
    {
       enemyStateManager.startChaseTimer = 3.0f;
       enemyStateManager.backToPatrol = 2.0f;
    }
}
