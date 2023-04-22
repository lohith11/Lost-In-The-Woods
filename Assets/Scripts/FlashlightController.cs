using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject lightObject;
    private Light _flashLight;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] public  float maxIntensity = 500f;
    [SerializeField] private float normalIntensity = 100f;
    [SerializeField] private float flickerProbability = 0.1f;
    [SerializeField] private float minFlickerIntensity = 0.5f;
    [SerializeField] private float maxFlickerIntensity = 1.0f;
    [SerializeField] private LayerMask bigBadLayer;
    public bool bigBadInRange;


    void Start()
    {
        _flashLight = lightObject.GetComponent<Light>();
        _flashLight.enabled = true;
    }

    void Update()
    {
        Debug.Log("The current flashlight intensity is : " + _flashLight.intensity);
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(lightObject.activeInHierarchy) 
            {
                lightObject.SetActive(false);
            }
            else if(!lightObject.activeInHierarchy)
            {
                lightObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, bigBadLayer);
        if(colliders.Length > 0 && lightObject.activeInHierarchy)
        {
            _flashLight.intensity = Mathf.Lerp(normalIntensity , maxIntensity , 1.0f);
        }
        else   
            _flashLight.intensity = Mathf.Lerp(maxIntensity , normalIntensity , 1.0f);


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

