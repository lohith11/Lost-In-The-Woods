using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyHealth : MonoBehaviour
{
    [ShowInInspector]public static float health = 100;

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
