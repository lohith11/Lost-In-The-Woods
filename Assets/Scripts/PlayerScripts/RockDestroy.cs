using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroy : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;

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
    }
}