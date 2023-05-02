using UnityEngine;

public class BossManager : MonoBehaviour
{
    public BlindBrute boss;
    [SerializeField] Brazier[] braziers;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            boss.AOEAttack();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        boss.DefaultValues();
        foreach (Brazier brazier in braziers)
        {
            brazier.TurnOff();
        }
    }

    public void TakeDamage()
    {
        Debug.Log("The boss took damage");
    }

    public void DealDamage()
    {
        Debug.Log("The boss dealt damage");
    }
}
