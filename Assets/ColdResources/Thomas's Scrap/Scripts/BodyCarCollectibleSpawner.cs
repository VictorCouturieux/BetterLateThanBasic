using System.Collections.Generic;
using UnityEngine;

public class BodyCarCollectibleSpawner : MonoBehaviour
{
    [Header("Setup if the generation is random")] 
    [SerializeField] private bool isBodyCarPartRandom;

    [Space] [Header("If the generation isn't random, assign manualy the item it loot below")] 
    [SerializeField] private GameObject forcedLoot;

    [Space] [Header("List of all body car parts at each rarities")]
    public List<GameObject> bodyCarParts = new List<GameObject>();

    [HideInInspector] public int choosenLoot;

    private void Start()
    {
        if (isBodyCarPartRandom) choosenLoot = Random.Range(0, bodyCarParts.Count);
        else for (int i = 0; i < bodyCarParts.Count; i++) if (forcedLoot == bodyCarParts[i]) choosenLoot = i;
    }
}