using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] float explosionDelay;
    [SerializeField] ParticleSystem explosionEffect;

    public void Explode() => StartCoroutine(Explosion());

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionDelay);

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Rock"))
            Explode();
    }
}
