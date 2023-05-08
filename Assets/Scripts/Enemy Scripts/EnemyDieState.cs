
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
        Debug.Log("Die state");
        AlertEnemies();
        enemyStateManager.enemyAnimController.CrossFade("Death_Anim", 0.2f);
        enemyStateManager.enemyHealth.health = 0;

    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }


    private void AlertEnemies()
    {
        Collider[] nearEnemies = Physics.OverlapSphere(enemyStateManager.enemyAgent.transform.position, enemyStateManager.detectRange);
        foreach (Collider enemies in nearEnemies)
        {
            var enemyManager = enemies.GetComponent<EnemyStateManager>();
            if (nearEnemies.Length > 0)
            {
                enemyManager.switchState(enemyManager.AlertState);
            }
        }
    }

}
