using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceAndRemoveBodyCarParts : MonoBehaviour
{
    [Header("place the parent object where to set body car parts")] 
    [SerializeField] private GameObject visual;
    
    [Space][Header("Settings")]
    [SerializeField] private float carSensibilityToHit = 50f;
    
    private Rigidbody carRigidbody;
    private List<GameObject> addedBodyCarParts = new List<GameObject>();

    private void Start()
    {
        carRigidbody = transform.GetComponent<Rigidbody>();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.transform.CompareTag("Player"))
        {
            float myMagnitude = carRigidbody.velocity.magnitude;
            float otherCarMagnitude = collision.transform.GetComponent<Rigidbody>().velocity.magnitude;
            
            //compare if your car have less magnitude than the other one, then if his magnitude is enough to make you 
            //lose a body car part
            if (otherCarMagnitude > myMagnitude)
                if (otherCarMagnitude - myMagnitude >= carSensibilityToHit / 2)
                    if (addedBodyCarParts.Count > 0)
                    {
                        int rnd = Random.Range(0, addedBodyCarParts.Count);
                        addedBodyCarParts[rnd].transform.parent = null;
                        addedBodyCarParts[rnd].GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
                    }
                    
        }
        else if(carRigidbody.velocity.magnitude >= carSensibilityToHit)
        {
            Debug.Log("hit un mur");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("BodyCarPart"))
        {
            //attach the body car part the the car
            Debug.Log("attach a boducar part");
        }
    }
}
