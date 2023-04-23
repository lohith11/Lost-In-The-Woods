using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColliders : MonoBehaviour
{
    public GameObject player;
    public Collider[] collidersToToggle;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        // Turn off colliders if the player is within a certain distance
        if (distanceToPlayer < 5f)
        {
            foreach (Collider collider in collidersToToggle)
            {
                collider.enabled = false;
            }
        }
        // Turn on colliders if the player is outside of that distance
        else
        {
            foreach (Collider collider in collidersToToggle)
            {
                collider.enabled = true;
            }
        }
    }
}
