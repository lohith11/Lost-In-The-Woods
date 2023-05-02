using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image bloodSplatter;
    public Image BackGroundBlood;
    public float hurtTimer;

    [ShowInInspector]
    public static float maxHealth = 100f;
    private float healthRegeneration;
    private Coroutine healthRegenerationStart;
    public float healthUpgrade;
    [ShowInInspector]
    public static float Health = 100f;
    public static float baseHealth;
    public float healthRegenerationValue;

    private void Start()
    {
        Health = maxHealth;
        baseHealth = maxHealth;
    }

    private void Update()
    {
        
    }

    private void UpdateHealth()
    {
        Color splatterAlpha = bloodSplatter.color;
        splatterAlpha.a = 1 - (Health / maxHealth);
        bloodSplatter.color = splatterAlpha;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Took damage");
        Health -= damage;
        StartCoroutine(HurtEffect());
        //UpdateHealth();
        if(Health <= 0)
        {
            PlayerDead();
        }
        else if(healthRegenerationStart != null)
        {
            StopCoroutine(healthRegenerationStart);
        }

        healthRegenerationStart = StartCoroutine(HealthRegeneration());
    }

    public void PlayerDead()
    {
        Health = 0;
        if(healthRegenerationStart!= null)
        {
            StopCoroutine(healthRegenerationStart);
        }
        Debug.Log("Player Dead");
    }

    private IEnumerator HurtEffect()
    {
        BackGroundBlood.enabled = true;
        //Audio
        yield return new WaitForSeconds(hurtTimer);
        BackGroundBlood.enabled = false;
    }

    private IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(3f);
        WaitForSeconds timeToWait = new WaitForSeconds(0.2f);

        while(Health < maxHealth)
        {
            Health += healthRegenerationValue;
            if(Health > maxHealth)
            {
                Health = maxHealth;
            }
            yield return timeToWait;
        }
        healthRegenerationStart = null;
    }
}
