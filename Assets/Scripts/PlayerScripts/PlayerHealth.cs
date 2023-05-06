using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image bloodSplatter;
    public Image BackGroundBlood;
    public float hurtTimer;
    public GameObject deadScreen;

    [ShowInInspector]
    public static float maxHealth = 100f;
    private Coroutine healthRegenerationStart;
    public float healthUpgrade;
    [ShowInInspector]
    public static float Health = 100f;
    public static float baseHealth;
    public float healthRegenerationValue;
    public AudioClip[] playerHurtSound;
    private AudioSource playerAudioSource;
    private static bool isPlayerDead;
    private PlayerStateMachine playerStateMachine;
    public AudioClip deathSound;
    private void Start()
    {
        isPlayerDead = false;
        if(deadScreen!= null)
        {
            deadScreen.SetActive(false);
        }
        playerAudioSource = GetComponent<AudioSource>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        Health = maxHealth;
        baseHealth = maxHealth;
        BlindBrute.bossDamage += TakeDamage;
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
        UpdateHealth();
        if (Health <= 0)
        {
            isPlayerDead = true;
            PlayerDead();
        }
        else if (healthRegenerationStart != null)
        {
            StopCoroutine(healthRegenerationStart);
        }

        healthRegenerationStart = StartCoroutine(HealthRegeneration());
    }
    public void TakeDamage(object sender , dealDamageEventArg e)
    {
        Health -= e.damage;
    }

    public void PlayerDead()
    {
        Health = 0;
        if (healthRegenerationStart != null)
        {
            StopCoroutine(healthRegenerationStart);
        }
        StartCoroutine(PlayerDeath());
    }

    private IEnumerator HurtEffect()
    {
        if (isPlayerDead)
        {
            yield break;
        }
        BackGroundBlood.enabled = true;
        AudioClip audioClip = playerHurtSound[Random.Range(0, playerHurtSound.Length - 1)];
        playerAudioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(hurtTimer);
        BackGroundBlood.enabled = false;
    }

    private IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(3f);
        WaitForSeconds timeToWait = new WaitForSeconds(0.2f);

        while (Health < maxHealth)
        {
            Health += healthRegenerationValue;
            UpdateHealth();
            if (Health > maxHealth)
            {
                Health = maxHealth;
            }
            yield return timeToWait;
        }
        healthRegenerationStart = null;
    }

    private IEnumerator PlayerDeath()
    {
        playerStateMachine.playerAnimation.Play("Player_Dead");
        playerStateMachine.audioSource.PlayOneShot(deathSound);
        deathSound = null;
        //Camera Movement
        yield return new WaitForSeconds(3f);
        deadScreen.SetActive(true);
    }
}