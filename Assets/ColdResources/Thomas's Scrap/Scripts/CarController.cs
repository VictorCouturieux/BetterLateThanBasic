using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<Transform> tireTransforms;
    [Space]
    [Header("suspension setup")]
    [SerializeField] private float springStrength = 50;
    [SerializeField] private float springDamper = 5;
    [SerializeField] private float springOffsef = 1;
    
    [Space]
    [Header("steering setup")]
    
    [Space]
    [Header("acceleration setup")]

    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (tireTransforms.Count < 1) Debug.Log("tireTransforms is empty, please assign tires !");
    }

    private void Update()
    {
        RaycastHit tireRay;
        Vector3 downDir = transform.TransformVector(-transform.up);
        
        
        for (int i = 0; i < tireTransforms.Count; i++)
        {
            bool rayDidHit = Physics.Raycast(tireTransforms[i].position, downDir, out tireRay, Mathf.Infinity);
            if (rayDidHit)
            {
                Debug.DrawRay(tireTransforms[i].position, downDir * tireRay.distance, Color.red);
                
                //world-space direction of the spring force
                Vector3 springDir = tireTransforms[i].up;
                
                //world-space velocity of this tire
                Vector3 tireWorldVel = rb.GetPointVelocity(tireTransforms[i].position);
                
                //calculate offset from raycast
                float offset = springOffsef - tireRay.distance;
                
                //calculate velocity along the spring direction
                //note that springDir is a unit vector, so this return the magnitude of of tireWorldVel
                //as projected onto springDir
                float vel = Vector3.Dot(springDir, tireWorldVel);
                
                //calculate the magnitude of the dampened spring force
                float force = (offset * springStrength) - (vel * springDamper);
                
                //apply the force at the location of this tire, in the direction of the suspension
                rb.AddForceAtPosition(springDir * force,tireTransforms[i].position);
            }
        }
    }
}