using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(!isThrown)
        {
            rb.useGravity = false;
            bc.isTrigger = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this.gameObject, 3f);
            
        }
        //Save it in player prfs later
        else if(collision.collider.CompareTag("Collectable"))
        {
            collectable++;
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
