using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dealDamageEventArg : EventArgs
{
    public float damage;
}

public class dealDamageToUngaEventArgs : EventArgs
{
    public float damage;
    public Collider enemyCollider;
}
public class RockDestroy : MonoBehaviour
{
    public static event EventHandler<dealDamageToUngaEventArgs> dealDamage;

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

        // if (collision.gameObject.name == "mixamorig:Head")
        // {
        //     Debug.Log("Entering EnemyHead");
        //     dealDamage?.Invoke(this, new dealDamageToUngaEventArgs { damage = 100 , enemyCollider = collision.collider });
        //     Destroy(this.gameObject);
        // }

        // if (collision.gameObject.name == "UngaBunga_Boi")
        // {

        // }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Entering EnemyBody");
            dealDamage?.Invoke(this, new dealDamageToUngaEventArgs { damage = 50, enemyCollider = collision.collider });
            collision.gameObject.GetComponent<EnemyStateManager>().health -= 50;
            Destroy(this.gameObject);
        }
    }
}
