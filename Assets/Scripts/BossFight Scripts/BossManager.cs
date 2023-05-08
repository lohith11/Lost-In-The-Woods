using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static event EventHandler<dealDamageEventArg> rammingDamage;
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3,
        EndBattle,
    }

    private BlindBrute boss;
    [SerializeField] Brazier[] braziers;
    [SerializeField] float ramSpeed;
    [SerializeField] float ramAcceleration;

    private Stage stage;

    private void Awake()
    {
        stage = Stage.WaitingToStart;
    }
    void Start()
    {
        StartBattle();
        boss = FindObjectOfType<BlindBrute>();
        Barrel.explosiveDamage += BossBattle_OnDamaged;
        BlindBrute.endBossBattle += EndBattle;
        StartNextStage();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            RamTowardsBrazier();
        }
    }

    private void StartBattle()
    {
        foreach (Brazier brazier in braziers)
        {
            brazier.TurnOff();
        }
    }

    public void BossBattle_OnDamaged(object sender, dealDamageEventArg e)
    {
        switch (stage)
        {
            case Stage.Stage_1:
                if (boss.GetHealthNormalized() <= .75f)
                {
                    StartNextStage();
                }
                break;
            case Stage.Stage_2:
                if (boss.GetHealthNormalized() <= .5f)
                {
                    StartNextStage();
                }
                break;
            case Stage.Stage_3:
                if (boss.GetHealthNormalized() <= .25f)
                {
                    StartNextStage();
                }
                break;
        }
    }

    private void EndBattle(object sender, EventArgs e)
    {
        Debug.Log("Battle ended!");
    }

    private void StartNextStage()
    {
        switch (stage)
        {
            case Stage.WaitingToStart:
                stage = Stage.Stage_1;
                break;
            case Stage.Stage_1:
                stage = Stage.Stage_2;
                boss.MoveAttack();
                break;
            case Stage.Stage_2:
                stage = Stage.Stage_3;
                RamTowardsBrazier();
                break;
            case Stage.Stage_3:
                Debug.Log("Stage 3 entered");
                boss.AOEAttack();
                break;
        }
    }

    public void RamTowardsBrazier()
    {
        Debug.Log("Raming towards brazier");
        boss.agent.speed = ramSpeed;
        boss.agent.acceleration = ramAcceleration;
        boss.bossAnimator.Play("Ram");
        int targetBrazier = UnityEngine.Random.Range(1, braziers.Length - 1);
        boss.agent.SetDestination(braziers[targetBrazier].transform.position);
        braziers[targetBrazier].TurnOff();
        if (boss.isOiled)
        {
            rammingDamage?.Invoke(this, new dealDamageEventArg { damage = 100 });
        }
        else
        {
            rammingDamage?.Invoke(this, new dealDamageEventArg { damage = 70 });
        }


    }

}
