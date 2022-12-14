using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BodyCarPartsManager : MonoBehaviour
{
    [Header("place the parent object where to set body car parts")] 
    [SerializeField] private GameObject visual;

    [Space] [Header("Settings")] 
    [SerializeField] private float carSensibilityToHit = 50f;

    [HideInInspector] public int playerScore;

    private Rigidbody carRigidbody;
    private List<GameObject> addedBodyCarParts = new();
    private int[] nunberOfEachBodyCarPart = new int[5];
    private GameObject[] tireArrray = new GameObject[4];

    private void Start()
    {
        carRigidbody = transform.GetComponent<Rigidbody>();
        for (int i = 1; i < 5; i++) tireArrray[i - 1] = transform.GetChild(0).transform.GetChild(i).gameObject;
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
            {
                if (otherCarMagnitude - myMagnitude >= carSensibilityToHit / 2)
                    LoseARandomBodyCarPArt();
            }
            else if (collision.transform.CompareTag("Barrier") && carRigidbody.velocity.magnitude >= carSensibilityToHit) 
                LoseARandomBodyCarPArt();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BodyCarPart")) //trigger a loot box
        {
            //vibrate a little the gamepad when player drive on a collectible
            //StartCoroutine(StartAndStopVibration(1f, .25f));

            var currentCollectibleSpawner = other.GetComponent<BodyCarCollectibleSpawner>();
            bool isAlreadyOnCar = false;

            currentCollectibleSpawner.OnCollisionWithCar();
            
            foreach (GameObject element in addedBodyCarParts.Where(element =>
                         element.name == currentCollectibleSpawner
                             .bodyCarParts[currentCollectibleSpawner.choosenLoot]
                             .name))
                isAlreadyOnCar = true;

            if (!isAlreadyOnCar)
            {
                GameObject go = Instantiate(currentCollectibleSpawner.bodyCarParts[currentCollectibleSpawner.choosenLoot],
                    transform.position, quaternion.identity, visual.transform);
                
                go.transform.localRotation = Quaternion.Euler(0, 0, 0);
                go.name = currentCollectibleSpawner.bodyCarParts[currentCollectibleSpawner.choosenLoot].name;
                addedBodyCarParts.Add(go);

                if (go.name.Contains("Wheel"))
                    for (int i = 0; i < tireArrray.Length; i++) tireArrray[i].SetActive(false);
            }

            playerScore += 500;

            for (int i = 0; i < addedBodyCarParts.Count; i++)
            {
                if (addedBodyCarParts[i].name == currentCollectibleSpawner
                        .bodyCarParts[currentCollectibleSpawner.choosenLoot].name)
                    nunberOfEachBodyCarPart[i]++;
            }
        }
    }

    /*IEnumerator StartAndStopVibration(float lowF, float highF)
    {
        Gamepad.current.SetMotorSpeeds(lowF,highF);
        yield return new WaitForSeconds(.3f);
        Gamepad.current.SetMotorSpeeds(0,0);
    }*/
    
    private void LoseARandomBodyCarPArt()
    {
        //vibrate a lot the gamepad when player hit a fence
        //StartCoroutine(StartAndStopVibration(.5f, 1f));

        if (addedBodyCarParts.Count > 0)
        {
            int rnd = Random.Range(0, addedBodyCarParts.Count);
            addedBodyCarParts[rnd].transform.DetachChildren();
            addedBodyCarParts[rnd].GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
            Destroy(addedBodyCarParts[rnd], 2.5f);

            if (addedBodyCarParts[rnd].name.Contains("Wheel"))
                for (int i = 0; i < tireArrray.Length; i++)
                    tireArrray[i].SetActive(true);

            for (int i = 0; i < addedBodyCarParts.Count; i++)
            {
                if (addedBodyCarParts[i] == addedBodyCarParts[rnd])
                {
                    playerScore -= 500 * nunberOfEachBodyCarPart[i];
                    nunberOfEachBodyCarPart[i] = 0;
                }
            }

            addedBodyCarParts.RemoveAt(rnd);
        }
    }
}