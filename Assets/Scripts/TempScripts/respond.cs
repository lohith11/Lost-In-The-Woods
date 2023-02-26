using System.Collections;
using UnityEngine;

public class respond : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    public float soundCheckInterval = 1f;
    public float hearingRange = 10f;

    private Transform target;
    private bool isChasing = false;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CheckForSounds());
    }

    private void Update()
    {
        if (isChasing)
        {
            Chase();
        }
    }

    private IEnumerator CheckForSounds()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, hearingRange);
            foreach (Collider collider in colliders)
            {
                Listen sound = collider.GetComponent<Listen>();
                if (sound != null)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < sound.range)
                    {
                        isChasing = true;
                        target = collider.transform;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(soundCheckInterval);
        }
    }

    private void Chase()
    {
        Vector3 targetDirection = target.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
