using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyHealth : MonoBehaviour
{
    public  float health = 100;

    void Awake()
    {
       // RockDestroy.dealDamage += TakeDamage;
    }

    private void TakeDamage(object sender, dealDamageToUngaEventArgs e)
    {
        if(health != 0)
        {
            health -= e.damage;
        }
    }

    public void Damage(float damage)
    {
        Debug.Log("damage called " + damage);
        damage -= health;
    }

}
