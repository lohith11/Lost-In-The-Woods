using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrabolicJump : MonoBehaviour
{
   public Transform targetPosition;
    public float pounceHeight = 1.0f;
    public float pounceDuration = 1.0f;
    private Vector3 startPosition;
    private float startTime;

    private void Start()
    {
        startPosition = transform.position;
        startTime = Time.time;
    }

    private void Update()
    {
        float timeElapsed = Time.time - startTime;
        if (timeElapsed < pounceDuration)
        {
            float normalizedTime = timeElapsed / pounceDuration;
            float parabolicT = 1 - 4 * (normalizedTime - 0.5f) * (normalizedTime - 0.5f);
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, normalizedTime) + Vector3.up * parabolicT * pounceHeight;
        }
        else
        {
            transform.position = targetPosition.position;
        }
    }
}
