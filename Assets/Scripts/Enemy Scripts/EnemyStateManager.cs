using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    /// TODO:  make the variables private after they start working
    
    //* Reference to the enemy animator 
    public Animator enemyAnimController;

    //* Nav mesh agent
    public NavMeshAgent EnemyAgent;
 
    //* this holds the reference to the current state the enemy is in
    EnemyBaseState currentState; 

    public float sphereRaidus;
    public Transform centrePoint;

    
    #region EnemyStates

    public EnemyIdleState   IdleState   = new EnemyIdleState();
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyChaseState  chaseState  = new EnemyChaseState();
    public EnemyDieState    dieState    = new EnemyDieState();
    public EnemyAlertState  alertState  = new EnemyAlertState();

    #endregion

    void Start()
    {
        
        //*This sets the starting state of the enemy
        currentState = IdleState;  

        currentState.EnterState(this);
    }

    
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState = Enemy;
        Enemy.EnterState(this);
    }

    
}
