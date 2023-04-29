using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] float explosionDelay;
    [SerializeField] float explosionRange;
    //[SerializeField] ParticleSystem explosionEffect;
    [SerializeField] LayerMask bossLayer;
    [SerializeField] BossManager boss;

    public void Explode() => StartCoroutine(Explosion());

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange , bossLayer);
        if(colliders.Length > 0 )
        {
           // BossManager boss = GetComponent<BossManager>();
            if(boss == null)
            {
                Debug.Log("Boss not fond");
            }
            boss.TakeDamage();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Rock"))
            Explode();
        else if(other.gameObject.CompareTag("Player"))
            Explode();
    }
}
