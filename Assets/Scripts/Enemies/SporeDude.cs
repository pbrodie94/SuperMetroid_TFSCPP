using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeDude : MonoBehaviour
{
    [SerializeField] private GameObject spore;
    [SerializeField] private Transform mouth;

    [SerializeField] private int numberOfShots = 3;
    [SerializeField] private float shotInterval = 1;

    private float timeLastAttacked;
    private float timeLastShot;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (shotInterval <= 0)
        {
            shotInterval = 1;
        }

        if (numberOfShots <= 0)
        {
            numberOfShots = 3;
        }
    }

    public void Shoot()
    {
        int shots = 0;

        anim.SetBool("Open", true);
        timeLastShot = Time.time + 2;

        while (shots < numberOfShots)
        {
            if (Time.time >= timeLastShot + shotInterval)
            {
                timeLastShot = Time.time;
                ShootSpore();

                shots++;
            }
        }

        anim.SetBool("Open", false);

        timeLastAttacked = Time.time;
    }

    void ShootSpore()
    {
        Instantiate(spore, mouth.position, Quaternion.identity);
    }

    public float GetLastAttacked()
    {
        return timeLastAttacked;
    }
}
