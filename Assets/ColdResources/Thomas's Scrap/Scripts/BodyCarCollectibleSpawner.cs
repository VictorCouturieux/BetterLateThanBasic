using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCarCollectibleSpawner : MonoBehaviour
{
    [Header("Setup if the generation is random")] 
    [SerializeField] private bool isBodyCarPartRandom;

    [Space] [Header("If the generation isn't random, assign manualy the item it loot below")] 
    [SerializeField] private GameObject forcedLoot;

    [Space] [Header("Reusabilitye  setup")]
    [SerializeField] private bool isReusable;
    [SerializeField] private float delayToReappear;
    
    [Space] [Header("List of all body car parts at each rarities")]
    public List<GameObject> bodyCarParts = new List<GameObject>();
    
    [HideInInspector] public int choosenLoot;
    private BoxCollider collider;

    private void Start()
    {
        collider = transform.GetComponent<BoxCollider>();
        if (isBodyCarPartRandom) choosenLoot = Random.Range(0, bodyCarParts.Count);
        else for (int i = 0; i < bodyCarParts.Count; i++) if (forcedLoot == bodyCarParts[i]) choosenLoot = i;
    }

    public void OnCollisionWithCar()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        collider.enabled = false;
        if (isReusable) StartCoroutine(ShowLootBack());
    }

    IEnumerator ShowLootBack()
    {
        yield return new WaitForSeconds(delayToReappear);
        transform.GetChild(0).gameObject.SetActive(true);
        collider.enabled = true;
    }
}