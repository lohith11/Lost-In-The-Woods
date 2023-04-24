using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColliders : MonoBehaviour
{
    public GameObject player;
    public float colliderCheckRange;
    public Collider[] collidersToToggle;
    private void Start()
    {

        StartCoroutine(ToggleCollider());
    }

    void Update()
    {
        // foreach (Collider collider in collidersToToggle)
        // {
        //     float distanceToPlayer = Vector3.Distance(player.transform.position, collider.transform.position);
        //     Debug.Log("Distance to the player is " + distanceToPlayer);

        //     if (distanceToPlayer < 2f)
        //     {
        //         collider.enabled = true;
        //     }
        //     else
        //     {
        //         collider.enabled = false;
        //     }
        // }


    }

    private IEnumerator ToggleCollider()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, colliderCheckRange);
            if (colliders.Length > 0)
            {
                Debug.Log("Found!");
            }
            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
                
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
