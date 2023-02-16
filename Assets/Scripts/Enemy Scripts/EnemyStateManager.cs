using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    /// TODO:  make the variables private after they start working
    public static EnemyStateManager manager;
    public Animator enemyAnimController;

    public NavMeshAgent EnemyAgent;
 
    EnemyBaseState currentState; //* this holds the reference to the current state the enemy is in

    public float sphereRaidus; //* the radius in which the enemy patrols
    public Transform centrePoint; //* the point around which there is sphere radius
    public bool playerInRange = false;


    #region EnemyStates

    public EnemyIdleState   IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyChaseState  chaseState;
    public EnemyDieState    dieState;
    public EnemyAlertState  alertState;

    #endregion
    void Start()
    {
        manager = this;
        IdleState   = new EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        chaseState  = new EnemyChaseState(this);
        dieState    = new EnemyDieState(this);
        alertState  = new EnemyAlertState(this);

        switchState(IdleState);
    }

    
    void Update()
    {
        currentState.UpdateState();
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState?.ExitState();
        currentState = Enemy;
        Enemy.EnterState();
    }

    
}
