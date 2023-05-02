using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("< Health Visual >")]
    [Space(5)]
    public Image overlay;
    public float duration;
    public float speed;
    public float durationTimer;
    [Space(10)]

    [ShowInInspector]
    public static float maxHealth = 100;
    private float healthRegeneration;
    private Coroutine healthRegenerationStart;
    public float healthUpgrade;
    [ShowInInspector]
    public static float Health;
    public static float baseHealth;

    private PlayerStateMachine StateMachine;

    private void Start()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        Health = maxHealth;
        baseHealth = maxHealth;
    }

    private void Update()
    {
        HealthVisual();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Took damage"); //!
        Health -= damage;
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

    public void HealthVisual()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth);
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float Alpha = overlay.color.a;
                Alpha -= Time.deltaTime * speed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Alpha);
            }
        }
    }

    private IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(3f);
        WaitForSeconds timeToWait = new WaitForSeconds(0.2f);
        while(Health < maxHealth)
        {
            Health += Time.deltaTime;
            if(Health > maxHealth)
            {
                Health = maxHealth;
                yield return timeToWait;
            }
        }
    }
}
