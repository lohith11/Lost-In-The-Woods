using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float health;
    [SerializeField] float damage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            AOEAttack();
        }
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
            foreach(Collider brazier in braziers)
            {
                brazier.GetComponent<Brazier>().TurnOff();
            }
        }
    }
}
