using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] float damage;
    [SerializeField] LayerMask playerLayer;
    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, playerLayer);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Found someting"); //!
            PlayerHealth player = hitColliders[0].GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("Ready to deal damage"); //!
                player.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
