using UnityEngine;

public class BossManager : MonoBehaviour
{
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3,
    }

    private BlindBrute boss;
    [SerializeField] Brazier[] braziers;

    private Stage stage;

    private void Awake()
    {
        stage = Stage.WaitingToStart;
    }

    void Start()
    {
        boss = FindObjectOfType<BlindBrute>();
        Barrel.explosiveDamage += BossBattle_OnDamaged;
        StartNextStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            boss.AOEAttack();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            RamTowardsBrazier();
        }

        Debug.Log("The current stage is  : " + stage);
    }

    private void StartBattle()
    {
        //boss.DefaultValues();
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

    private void StartNextStage()
    {
        switch (stage)
        {
            case Stage.WaitingToStart:
                stage = Stage.Stage_1;
                break;
            case Stage.Stage_1:
                stage = Stage.Stage_2;
                RamTowardsBrazier();
                break;
            case Stage.Stage_2:
                stage = Stage.Stage_3;
                RamTowardsBrazier();
                break;
            case Stage.Stage_3:
                RamTowardsBrazier();
                break;
        }
    }

    private void RamTowardsBrazier()
    {
        boss.agent.speed = 20f;
        boss.agent.acceleration = 40f;
        boss.agent.SetDestination(braziers[Random.Range(0, braziers.Length - 1)].transform.position);
        Debug.Log("Array index is : " + Random.Range(0, braziers.Length));

    }

    public void DealDamage()
    {
        Debug.Log("The boss dealt damage");
    }
}
