using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void PlayerSpawn();
    public static event PlayerSpawn OnPlayerSpawn;

    private Transform player;

    public Transform spawnPoint;
    public Transform checkPoint;

    private SamusStatus samusStatus;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        samusStatus = player.GetComponent<SamusStatus>();
    }

    public void PlayerDie()
    {
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    public void RespawnPlayer()
    {
        if (!checkPoint)
        {
            player.position = spawnPoint.position;
        }
    }

    public void PlayerSpawned()
    {
        OnPlayerSpawn();
    }
}
