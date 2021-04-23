using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphballBomb : MonoBehaviour
{
    [SerializeField] private float fragTime = 3;

    float timeSpawned = 0;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (fragTime <= 0)
        {
            fragTime = 3;
        }

        timeSpawned = Time.time;
    }

    private void Update()
    {
        if (Time.time >= timeSpawned + fragTime)
        {
            Explode();
        }
    }

    void Explode()
    {
        //Damage enemies

        //Morphball Jump

        anim.SetTrigger("Explode");

        Destroy(gameObject, 0.4f);
    }
}
