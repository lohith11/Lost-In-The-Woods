using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    /// TODO:  get the animator to play the animations
    public Animator enemyAnimController;
    public SphereCollider _alertRadius;
    public bool alert;
    EnemyBaseState currentState; //* this holds the reference to the current state the enemy is in
    public EnemyIdleState   IdleState   = new EnemyIdleState();
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyChaseState  chaseState  = new EnemyChaseState();
    public EnemyDieState    dieState    = new EnemyDieState();
    public EnemyAlertState  alertState  = new EnemyAlertState();
    void Start()
    {
        currentState = IdleState; //* Enemy starting state

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState = Enemy;
        Enemy.EnterState(this);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            alert = true;
        }
    }
}
