using UnityEngine;


public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        Debug.Log("Chasing!"); //! delete the debug
    }


    public override void UpdateState()
    {
        

    }

    
    public override void ExitState()
    {
       
    }

}
