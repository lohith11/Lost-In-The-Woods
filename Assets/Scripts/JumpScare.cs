using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    [SerializeField] Transform pointA, pointB;
    [SerializeField] float speed;
    [SerializeField] bool isMoving;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(MoveObject());
        }
    }

    IEnumerator MoveObject()
    {
        float distance = Vector3.Distance(pointA.position, pointB.position);
        float totalTime = distance / speed;
        float currentTime = 0;

        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / totalTime;
            transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
            yield return null;
        }

        isMoving = false;
        this.gameObject.SetActive(false);
    }
}
