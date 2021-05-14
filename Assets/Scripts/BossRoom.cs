using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    private bool bossDefeated = false;
    private bool battling = false;

    [SerializeField] private SporeSpawn boss;
    [SerializeField] private SporeDude[] sporeDudes;
    [SerializeField] private float shootInterval = 5;

    [SerializeField] private Door[] doors;

    private float timeLastshot = 0;
 
    protected override void Start()
    {
        base.Start();

        if (!boss)
            GameObject.Find("SporeSpawn").GetComponent<SporeSpawn>();

        if (shootInterval <= 0)
        {
            shootInterval = 5;
        }
    }

    private void Update()
    {
        if (!bossDefeated && battling)
        {

            if (Time.time >= timeLastshot + shootInterval)
            {
                int rand = Random.Range(0, sporeDudes.Length - 1);

                StartCoroutine(sporeDudes[rand].Shoot());

                timeLastshot = Time.time;
            }
        }
    }

    public override void SetUpRoom()
    {
        base.SetCameraBounds();

        if (!bossDefeated)
        {
            //Start boss battle
            StartCoroutine(boss.BeginBattle());

            timeLastshot = Time.time;

            for (int i = 0; i < doors.Length - 1; i++)
            {
                doors[i].SetDoorLocked(true);
            }

            battling = true;
        }
    }

    public override void DestroyEntities()
    {
        if (!bossDefeated)
            boss.ResetBoss();

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");

        foreach (GameObject p in pickups)
        {
            Destroy(p);
        }

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject proj in projectiles)
        {
            Destroy(proj);
        }
    }
}
