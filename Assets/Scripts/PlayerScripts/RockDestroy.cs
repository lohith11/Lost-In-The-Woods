using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this.gameObject, 3f);
        }
    }
}
