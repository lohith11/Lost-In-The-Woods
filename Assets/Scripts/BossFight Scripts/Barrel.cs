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
    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R pressed");
            Explode();
        }
        #endif
    }

    public void Explode() => StartCoroutine(Explosion());

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, bossLayer);
        if (colliders.Length > 0)
        {
            explosiveDamage.Invoke(this, new dealDamageEventArg { damage = 50 });
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Rock"))
            Explode();
    }
}
