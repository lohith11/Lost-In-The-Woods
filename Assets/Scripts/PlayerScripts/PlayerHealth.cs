using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("< Health Visual >")]
    [Space(5)]
   // public Image overlay;
    //public float duration;
   // public float speed;
   // public float durationTimer;
    [Space(10)]

    public static float maxHealth;
    public float healthUpgrade;
    public static float Health = 70;
    private float baseHealth;

    private PlayerStateMachine StateMachine;

    private void Start()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        Health = maxHealth;
        baseHealth = maxHealth;
    }

    private void Update()
    {
        maxHealth = baseHealth + (StateMachine.herbs * 10);
       // Health = maxHealth;
        if(Health <= 0)
        {
            Health = 0;
        }

        if(Health == 0)
        {
            //DeadState
        }
      //  HealthVisual();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Took damage"); //!
        Health -= damage;
    }

    // public void HealthVisual()
    // {
    //     Health = Mathf.Clamp(Health, 0, maxHealth);
    //     if(overlay.color.a > 0)
    //     {
    //         durationTimer += Time.deltaTime;
    //         if(durationTimer > duration) 
    //         {
    //             float Alpha = overlay.color.a;
    //             Alpha -= Time.deltaTime * speed;
    //             overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Alpha);
    //         }
    //     }
    // }
    
}
