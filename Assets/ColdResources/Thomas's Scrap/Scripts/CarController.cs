using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// this script is a physic based car controller made by me (Thomas Boulanger) inspired by this video :
/// https://www.youtube.com/watch?v=CdPYlj5uZeI
/// this script need a rigidbody and a list of transform to simulate tire position
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("place front wheels then rear wheel in this order inside the list")]
    [SerializeField] private List<Transform> tireTransforms;
    
    [Space] [Header("Suspension setup")]
    [SerializeField] private float springStrength = 50;
    [SerializeField] private float springDamper = 5;
    [SerializeField] private float springOffsef = .65f;

    [Space] [Header("Steering setup")] 
    [SerializeField] private float frontGripFactor = 40;
    [SerializeField] private float rearGripFactor = 40;
    [SerializeField] private float tireMass = .01f;

    [Space] [Header("Acceleration setup")] 
    [SerializeField] private float carTopSpeed = 50;
    [SerializeField] private float speed = 20;
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float drag = 2;

    [Space] [Header("Rotation angle setup")] 
    [SerializeField] private float maxWheelAngle = 30;
    
    [Space] [Header("Others setup")] 
    [SerializeField] private float carMass = 30;
    
    private Rigidbody carRigidbody;
    private float forwardInput;
    private float rightInput;
    private float tireGripFactor;
    private float raycastDistance;
    
    //gravity settings
    private const float GRAVITATIONAL_ACCELERATION = 9.81f;
    
    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        if (tireTransforms.Count < 1) Debug.Log("tireTransforms is empty, please assign tires !");
        raycastDistance = springOffsef + 1;
    }

    private void Update()
    {
        //loop for each tire
        for (int i = 0; i < tireTransforms.Count; i++)
        {
            Vector3 downDir = -Vector3.up;
            bool rayDidHit = Physics.Raycast(tireTransforms[i].position, downDir, out RaycastHit tireRay, raycastDistance);
            
            //apply different grip on front and rear tire, also rotate front tire depending on player inputs
            if (i < 2)
            {
                tireTransforms[i].localRotation = Quaternion.Euler(0,rightInput * maxWheelAngle,0);
                tireGripFactor = frontGripFactor;
            }
            else  tireGripFactor = rearGripFactor;
            
            //suspension force
            if (rayDidHit)
            {
                if (Vector3.Angle(tireTransforms[i].up,tireRay.normal) > 60) return;

                //world-space direction of the spring force
                Vector3 springDir = tireTransforms[i].up;
                
                //world-space velocity of this tire
                Vector3 tireWorldVel = carRigidbody.GetPointVelocity(tireTransforms[i].position);
                
                //calculate offset from raycast
                float offset = springOffsef - tireRay.distance;
                
                //calculate velocity along the spring direction
                //note that springDir is a unit vector, so this return the magnitude of tireWorldVel
                //as projected onto springDir
                float vel = Vector3.Dot(springDir, tireWorldVel);
                
                //calculate the magnitude of the dampened spring force
                float force = (offset * springStrength) - (vel * springDamper);
                
                //apply the force at the location of this tire, in the direction of the suspension
                carRigidbody.AddForceAtPosition(springDir * force,tireTransforms[i].position);
                
                Debug.DrawRay(tireTransforms[i].position, springDir * vel, Color.green);
            }
            
            //steering force
            if (rayDidHit)
            {
                //world-space direction of the spring force
                Vector3 steeringDir = tireTransforms[i].right;
                
                //world-space velocity of the suspension
                Vector3 tireWorldVel = carRigidbody.GetPointVelocity(tireTransforms[i].position);

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
                carRigidbody.AddForceAtPosition(steeringDir * tireMass * desiredAccel, tireTransforms[i].position);

                Debug.DrawRay(tireTransforms[i].position, steeringDir * steeringVel, Color.red);
                
                //FMOD SOUND INTEGRATION STEERING/DRIFT value -> (steeringDir * steeringVel).magnitude
                float FmodDrift = (steeringDir * steeringVel).magnitude;
                if (i == 0) ; //place your code here before ';'
            }
            
            //acceleration / braking
            //note that in that case the car is a traction, test if i > 2 to change the car to propulsion
            if (rayDidHit && i < 2)
            {
                //world-space direction of the acceleration/braking force
                Vector3 accelDir = tireTransforms[i].forward;

                //acceleration torque
                if (Mathf.Abs(forwardInput) > 0)
                {
                    carRigidbody.drag = .3f;
                    //forward speed of the car (in the direction of driving)
                    float carSpeed = Vector3.Dot(transform.forward, carRigidbody.velocity);

                    //normalized car speed
                    float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

                    //aviable torque
                    float aviableTorque = powerCurve.Evaluate(normalizedSpeed) * forwardInput;

                    carRigidbody.AddForceAtPosition(accelDir * aviableTorque * speed, tireTransforms[i].position);

                    Debug.DrawRay(tireTransforms[i].position, accelDir * carSpeed, Color.blue);
                    
                    //FMOD SOUND INTEGRATION ACCELERATION/SPEED value -> (accelDir * aviableTorque * speed).magnitude
                    float FmodAccel = (accelDir * aviableTorque * speed).magnitude;
                    if (i == 0) ; //place your code here before ';'
                }
                else
                {
                    carRigidbody.drag = drag;
                }
            }

            //if raycast didn't hit the ground, apply gravity on car at each tire position that didn't hit the ground
            if (!rayDidHit)
            {
                // Calculate the gravitational force acting on the object (in Newtons)
                float gravitationalForce = GRAVITATIONAL_ACCELERATION * carMass;

                // Update the acceleration of the object using the gravitational force
                Vector3 acceleration = new Vector3(0, -gravitationalForce, 0);

                // Update the velocity of the object using the current acceleration
                Vector3 velocity = (carRigidbody.velocity + acceleration) * Time.deltaTime;
                
                carRigidbody.AddForceAtPosition(velocity, tireTransforms[i].position);

                Debug.DrawRay(tireTransforms[i].position, velocity, Color.yellow);
            }
        }
    }

    
    //reading player Input with the Unity New Input System
    private void OnMoveForward(InputValue value) => forwardInput = value.Get<float>();
    private void OnMoveRight(InputValue value) => rightInput = value.Get<float>();
    
}