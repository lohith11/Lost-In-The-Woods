using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] float damage;
    [SerializeField] LayerMask playerLayer;
    [SerializeField]private PlayerHealth playerHealth;

    public void DealDamageToPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, playerLayer);

        if (hitColliders.Length > 0)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

}
