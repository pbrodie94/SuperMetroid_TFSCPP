using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    private bool bossDefeated = false;
    private bool battling = false;

    [SerializeField] private SporeSpawn boss;
    [SerializeField] private SporeDude[] sporeDudes;
    [SerializeField] private float shootInterval = 2;

    [SerializeField] private Door[] doors;

    private MusicManager musicManager;

    private float timeLastshot = 0;
 
    protected override void Start()
    {
        base.Start();

        if (!boss)
            GameObject.Find("SporeSpawn").GetComponent<SporeSpawn>();

        musicManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MusicManager>();

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

            musicManager.TransitionMusic(1);

            timeLastshot = Time.time;

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].SetDoorLocked(true);
            }

            battling = true;
        }
    }

    public override void DestroyEntities()
    {
        if (!bossDefeated)
        {
            musicManager.TransitionMusic(0);

            boss.ResetBoss();
        }

        battling = false;

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetDoorLocked(false);
        }

        base.DestroyEntities();
    }

    public void BossDefeated()
    {
        bossDefeated = true;

        musicManager.TransitionMusic(0);

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetDoorLocked(false);
        }
    }
}
