using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    private Light _flashLight;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float maxIntensity = 500f;
    [SerializeField] private float normalIntensity = 100f;
    [SerializeField] private float flickerProbability = 0.1f;
    [SerializeField] private float minFlickerIntensity = 0.5f;
    [SerializeField] private float maxFlickerIntensity = 1.0f;
    [SerializeField] private LayerMask bigBadLayer;
    public bool bigBadInRange;
    public bool flashLightTurnedOn;


    void Start()
    {
        _flashLight = GetComponent<Light>();
        _flashLight.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(_flashLight.enabled) 
            {
                _flashLight.enabled = false;
            }
            else
            {
                _flashLight.enabled = true;
            }
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, bigBadLayer);
        if (colliders.Length > 0)
            bigBadInRange = true;
        else
            bigBadInRange = false;
        if(bigBadInRange)
            _flashLight.intensity = maxIntensity;
        else
            _flashLight.intensity = normalIntensity;
    }


    void flicker()
    {

        if (Random.value < flickerProbability)
        {
            float flickerIntensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
            _flashLight.intensity *= flickerIntensity;
        }
        else
        {
            _flashLight.intensity = 0f;
        }


    }



   

}

