using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public static event EventHandler<dealDamageEventArg> explosiveDamage;
    [SerializeField] float explosionDelay;
    [SerializeField] float explosionRange;
    [SerializeField] LayerMask bossLayer;
    public GameObject fire;

    private void Start()
    {
        fire.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            explosiveDamage.Invoke(this, new dealDamageEventArg { damage = 50 });
        }
    }

    public void Explode() => StartCoroutine(Explosion());

    IEnumerator Explosion()
    {
        fire.SetActive(true);
        yield return new WaitForSeconds(explosionDelay);
        Debug.Log("Barrel Exploded");
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, bossLayer);
        if (colliders.Length > 0)
        {
            if(explosiveDamage != null)
            {
                explosiveDamage.Invoke(this, new dealDamageEventArg { damage = 50 });
            }
            gameObject.SetActive(false);
        }
    }
}
