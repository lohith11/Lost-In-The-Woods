using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    //public GameObject player;
    public Transform target;
    public float speed = 1f;

    void Update()
    {
        Debug.Log("Update");
        // Calculate the direction from this object to the target object
        Vector3 direction = target.position - transform.position;

        // Normalize the direction vector to get a unit vector
        direction.Normalize();

        // Move this object towards the target object at the given speed
        transform.position += direction * speed * 5.0f;
    }
}
