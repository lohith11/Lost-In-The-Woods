using UnityEngine;


public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Chasing!"); //! delete the debug
    }

    public override void UpdateState(EnemyStateManager Enemy)
    {
        throw new System.NotImplementedException();
    }
}
