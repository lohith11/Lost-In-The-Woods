using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    /// TODO:  make the variables private after they start working
    public Animator enemyAnimController;

    public NavMeshAgent EnemyAgent;
 
    EnemyBaseState currentState; //* this holds the reference to the current state the enemy is in

    public float sphereRaidus; //* the radius in which the enemy patrols
    public Transform centrePoint; //* the point around which there is sphere radius
    [HideInInspector] public bool playerInRange;
    [HideInInspector] public bool playerLost;

    #region EnemyStates

    public EnemyIdleState   IdleState   = new EnemyIdleState();
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyChaseState  chaseState  = new EnemyChaseState();
    public EnemyDieState    dieState    = new EnemyDieState();
    public EnemyAlertState  alertState  = new EnemyAlertState();

    #endregion

    void Start()
    {

        currentState = IdleState;   //*This sets the starting state of the enemy
        LineOfSight.losInstance.OnPlayerFound +=  OnPlayerFound;
        LineOfSight.losInstance.OnlostPlayer  +=  OnPlayerLost;
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

    public void OnPlayerFound(object sender, EventArgs e)
    {
        Debug.Log("Player found"); //!
        playerInRange = true;

    }

    public void OnPlayerLost(object sender, EventArgs e)
    {
        Debug.Log("Player lost!"); //!
        playerLost  = true;
    }
}
