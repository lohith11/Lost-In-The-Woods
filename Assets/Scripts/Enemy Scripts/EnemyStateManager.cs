using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    // TODO:  make the variables private after they start working
    public static EnemyStateManager manager;
    public Animator enemyAnimController;
    public NavMeshAgent EnemyAgent;
 
    //* The current state that enemy is in 
    EnemyBaseState currentState; 

    //* The radius in which the enemy patrols
    public float sphereRaidus; 
    //* The center point around whcich the patrol shphere is drawn 
    public Transform centerPoint; 
    public bool playerInRange = false;
    [Range(10,50)]
    public float hearingRange = 10f;
    public float soundCheckInterval = 1f;


    #region EnemyStates

    public EnemyIdleState   IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyChaseState  ChaseState;
    public EnemyDieState    DieState;
    public EnemyAlertState  AlertState;

    #endregion
    void Start()
    {
        manager = this;
        IdleState   = new   EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        ChaseState  = new  EnemyChaseState(this);
        DieState    = new    EnemyDieState(this);
        AlertState  = new  EnemyAlertState(this);

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
