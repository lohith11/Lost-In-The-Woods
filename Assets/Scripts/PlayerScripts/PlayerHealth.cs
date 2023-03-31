using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor;
using UnityEditor.U2D;
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


    public float maxHealth;
    public float healthUpgrade;
    public float Health;


    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        if()
        if(Health <= 0)
        {
            Health = 0;
        }

        if(Health == 0)
        {

        }
        HealthVisual();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void HealthVisual()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth);
        if(overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if(durationTimer > duration) 
            {
                float Alpha = overlay.color.a;
                Alpha -= Time.deltaTime * speed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Alpha);
            }
        }
    }
    
}
