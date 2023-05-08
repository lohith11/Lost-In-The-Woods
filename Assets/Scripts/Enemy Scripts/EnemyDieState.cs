
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
        Debug.Log("Die state");
        enemyStateManager.enemyAnimController.Play("Death_Anim");
    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}
