using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindBrute : MonoBehaviour
{

    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] float damage;
    [SerializeField] float range;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage()
    {
        Debug.Log("The boss took damage");
    }

    public void DealDamage()
    {
        Debug.Log("The boss dealt damage");
    }

    public void AOEAttack()
    {
        Collider[] braziers = Physics.OverlapSphere(transform.position, range);
        if(braziers.Length > 0)
        {
            Debug.Log("Entered if");
            foreach(Collider brazier in braziers)
            {
                Debug.Log("Entered for each");
                brazier.GetComponent<Brazier>().TurnOff();
            }
        }
    }
}
