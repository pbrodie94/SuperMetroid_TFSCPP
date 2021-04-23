using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] pickups;

    private void Start()
    {
        SpawnRandomPickups();
    }

    void SpawnRandomPickups()
    { 
        foreach (Transform child in transform)
        {
            int rand = Random.Range(0, pickups.Length - 1);

            GameObject go = pickups[rand];

            Instantiate(go, child.position, Quaternion.identity);
        }
    }
}
