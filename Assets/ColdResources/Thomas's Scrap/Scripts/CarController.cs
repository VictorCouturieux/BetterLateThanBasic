using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("place front wheels then rear wheel in this order inside the list")]
    [SerializeField] private List<Transform> tireTransforms;
    [Space]
    [Header("suspension setup")]
    [SerializeField] private float springStrength = 50;
    [SerializeField] private float springDamper = 5;
    [SerializeField] private float springOffsef = 1;

    [Space] 
    [Header("steering setup")] 
    [SerializeField] private float frontGripFactor = .30f;
    [SerializeField] private float rearGripFactor = .5f;
    [SerializeField] private float tireMass = 1;
    [Space]
    [Header("acceleration setup")]

    
    private Rigidbody rb;
    private Vector3 moveInput;
    private float tireGripFactor;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (tireTransforms.Count < 1) Debug.Log("tireTransforms is empty, please assign tires !");
    }

    private void Update()
    {
        for (int i = 0; i < tireTransforms.Count; i++)
        {
            Vector3 downDir = transform.TransformVector(-transform.up);
            bool rayDidHit = Physics.Raycast(tireTransforms[i].position, downDir, out RaycastHit tireRay, Mathf.Infinity);
            
            //suspension force
            if (rayDidHit)
            {
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
                
                Debug.DrawRay(tireTransforms[i].position, springDir * vel, Color.green);
            }
            
            if (i <= 2) tireGripFactor = frontGripFactor;
            else  tireGripFactor = rearGripFactor;
            
            //steering force
            if (rayDidHit)
            {
                //world-space direction of the spring force
                Vector3 steeringDir = tireTransforms[i].right;
                
                //world-space velocity of the suspension
                Vector3 tireWorldVel = rb.GetPointVelocity(tireTransforms[i].position);

                //what is the tire's velocity in the steering direction?
                //note that steeringDir is a unit vector, so this return the magnitude of of tireWorldVel
                //as projected onto steeringDir
                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
                
                //the change in velocity that we're looking for is -steeringVel * gripFactor
                //gripFactor is in range 0 -1, 0 means no grip, 1 means full grip
                float desiredeVelChange = -steeringVel * (tireGripFactor / 100);
                
                //turn change in velocity into an acceleration (acceleration = change in velocity / time)
                //this will produce the acceleration necessary to change the velocity by desiredeVelChange in 1 physics step 
                float desiredAccel = desiredeVelChange / Time.fixedDeltaTime;
                
                //Force = Mass * Acceleration, so multiply by the mass of the tire and apply it as a force
                rb.AddForceAtPosition(steeringDir * tireMass * desiredAccel, tireTransforms[i].position);

                //Debug.DrawRay(tireTransforms[i].position, downDir * tireRay.distance, Color.red);
            }
        }
    }
    
    
    private void OnMove(InputValue value)
    {
        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }
}