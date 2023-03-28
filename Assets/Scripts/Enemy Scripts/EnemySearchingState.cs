using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    public EnemySearchingState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
       // enemyStateManager.enemyAnimController.SetBool("SoundInRange",true);
        // Debug.LogError("Enter the state");
        //enemyStateManager.enemyAnimController.Play("Finding_Anim");
        enemyStateManager.SearchForPlayer();
    }

    public override void UpdateState()
    {
      //  enemyStateManager.enemyAgent.SetDestination(enemyStateManager.soundPosition);
       // enemyStateManager.enemyAnimController.SetBool("SoundInRange",false);
        //searchForPlayer();
    }

    public override void ExitState()
    {
       // enemyStateManager.searchForPlayer = 1.5f;
        
        Debug.LogError("Exit the state");
    }
}

//     private void searchForPlayer()
//     {
//         // if(enemyStateManager.PlayerInRange)
//         // {
//         //     enemyStateManager.switchState(enemyStateManager.AlertState);
//         // }
//         if (!enemyStateManager.PlayerInRange)
//         {
//             enemyStateManager.searchForPlayer -= Time.deltaTime;
//         }

//         if (enemyStateManager.searchForPlayer <= 0)
//         {
            
//             enemyStateManager.switchState(enemyStateManager.PatrolState);
            
//             if(enemyStateManager.isWayPointPatrol)
//             {
//                 enemyStateManager.enemyAgent.SetDestination(enemyStateManager.nextLocation);
//             }
//         }
//     }
// }
