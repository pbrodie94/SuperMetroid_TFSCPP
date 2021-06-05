using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void PlayerSpawn();
    public static event PlayerSpawn OnPlayerSpawn;

    [SerializeField] private Room currentRoom;

    private Transform player;

    public Transform startSpawnPoint;
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
        Room spawnRoom;
        Transform spawnPoint;

        if (checkPoint)
        {
            spawnPoint = checkPoint;
            spawnRoom = checkPoint.gameObject.GetComponentInParent<Room>();
        } else
        {
            spawnPoint = startSpawnPoint;
            spawnRoom = startSpawnPoint.gameObject.GetComponentInParent<Room>();
        }

        spawnRoom.SetSpawnRoom(currentRoom, spawnPoint);
    }

    public void PlayerSpawned()
    {
        OnPlayerSpawn();
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }
}
