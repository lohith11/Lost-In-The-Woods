using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static float health;

    void Awake()
    {
        RockDestroy.dealDamage += TakeDamage;
    }

    private void TakeDamage(object sender, dealDamageEventArg e)
    {
        if(health != 0)
        {
            health -= e.damage;
        }
    }

}
