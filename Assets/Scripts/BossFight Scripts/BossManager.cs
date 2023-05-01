using UnityEngine;

public class BossManager : MonoBehaviour
{
    public BlindBrute boss;
  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            boss.AOEAttack();
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
