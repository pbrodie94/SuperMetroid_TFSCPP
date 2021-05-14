using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDropper : MonoBehaviour
{
    bool dropped = false;

    [SerializeField] private float dropLifetime = 10;

    [Range(0, 100)]
    [SerializeField] private int dropProbability;
    [Range(0, 100)]
    [SerializeField] private int rareDropProbability;

    [Header("Pickups")]
    [SerializeField] private GameObject[] pickups;
    [SerializeField] private GameObject[] rarePickups;

    private void Start()
    {
        if (dropProbability <= 0)
        {
            dropProbability = 30;
        }
    }

    public void DropPickup()
    {
        if (!dropped)
        {
            if (pickups.Length <= 0)
                return;

            float rand = Random.Range(0, 10000);

            if (rand < ((double)dropProbability / 100f) * 10000f)
            {
                GameObject drop = pickups[0];

                //Will drop
                if (rareDropProbability > 0 && rarePickups.Length > 0)
                {
                    rand = Random.Range(0, 10000);

                    if (rand < ((double)rareDropProbability / 100f) * 10000f)
                    {
                        //Drop rare pickup
                        int i = Random.Range(0, rarePickups.Length);
                        drop = rarePickups[i];

                    } else
                    {
                        int i = Random.Range(0, pickups.Length);
                        drop = pickups[i];
                    }
                }
                else
                {
                    int i = Random.Range(0, pickups.Length);
                    drop = pickups[i];
                }

                GameObject go = Instantiate(drop, transform.position, Quaternion.identity);
                PickUp pu = go.GetComponent<PickUp>();
                pu.SetLifetime(dropLifetime);
            }

            dropped = true;
        }
    }
}
