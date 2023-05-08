using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.CrossFade("Walking_Anim" ,0.05f);
        enemyStateManager.alertText.enabled = true;
        enemyStateManager.alertText.text = "Alert!";
        enemyStateManager.enemyAgent.speed = enemyStateManager.alertSpeed;
        enemyStateManager.PlayAlertEffect();
    }

    public override void UpdateState()
    {
        enemyStateManager.transform.LookAt(enemyStateManager.transform.position, enemyStateManager.playerRef.transform.position);
        if (enemyStateManager.PlayerInRange)
        {
            enemyStateManager.startChaseTimer -= Time.deltaTime;
            enemyStateManager.backToPatrol = 2.0f;
        }
        if (!enemyStateManager.PlayerInRange)
        {
            enemyStateManager.backToPatrol -= Time.deltaTime;
        }

        else if (enemyStateManager.startChaseTimer <= 0)
        {
            enemyStateManager.switchState(enemyStateManager.ChaseState);
        }

        if (enemyStateManager.SoundInRange)
        {
            enemyStateManager.switchState(enemyStateManager.SearchState);
        }

        if (enemyStateManager.backToPatrol <= 0)
        {
            if (enemyStateManager.isWayPointPatrol)
            {
                enemyStateManager.enemyAgent.SetDestination(enemyStateManager.nextLocation);
            }
            enemyStateManager.switchState(enemyStateManager.PatrolState);
        }

    }

    public override void ExitState()
    {
        enemyStateManager.startChaseTimer = 3.0f;
        enemyStateManager.backToPatrol = 2.0f;
        enemyStateManager.alertText.enabled = false;
    }
}
