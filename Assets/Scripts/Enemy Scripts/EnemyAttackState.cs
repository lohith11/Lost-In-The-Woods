using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Attack_Anim");
    }


    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }
}
