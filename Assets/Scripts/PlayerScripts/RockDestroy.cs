using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dealDamageEventArg : EventArgs
{
    public float damage;
}

public class RockDestroy : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;
    public int collectable = 0;

    public bool isThrown = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (!isThrown)
        {
            rb.useGravity = false;
            bc.isTrigger = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this.gameObject, 3f);

        }

        //to do :- visual reprensentation of collectables
        //Save it in player prfs later
        if (collision.collider.CompareTag("Collectable"))
        {
            collectable++;
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyStateManager>().health -= 50;
            Destroy(this.gameObject);
        }
    }
}
