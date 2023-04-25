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
    public static event EventHandler<dealDamageEventArg> dealDamage;

    [SerializeField] private string headDamage;
    [SerializeField] private string bodyDamage;

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
        if(!isThrown)
        {
            rb.useGravity = false;
            bc.isTrigger = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground") || collision.collider.CompareTag("EnemyHead") || collision.collider.CompareTag("EnemyBody"))
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyHead"))
        {
            Debug.Log("Entering EnemyHead");
            dealDamage?.Invoke(this, new dealDamageEventArg { damage = 100 });
            Destroy(this.gameObject, 2f);
        }

        if (other.gameObject.CompareTag("EnemyBody"))
        {
            Debug.Log("Entering EnemyBody");
            dealDamage?.Invoke(this, new dealDamageEventArg { damage = 50 });
        }
    }
}
