using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("Sets the X axis bounds for camera movement")]
    [SerializeField] private Vector2 xAxisBounds = Vector2.zero;
    [Tooltip("Sets the y axis bounds for camera movement")]
    [SerializeField] private Vector2 yAxisBounds = Vector2.zero;

    [SerializeField] private EnemySpawnPoint[] spawnPoints;

    private CameraFollow2D cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraFollow2D>();
    }

    public void SetCameraBounds()
    {
        cam.SetCameraBounds(xAxisBounds, yAxisBounds);
    }

    public void SpawnEntities()
    {
        if (spawnPoints.Length > 0)
        {
            foreach (EnemySpawnPoint sp in spawnPoints)
            {
                sp.SpawnEnemy();
            }
        }
    }

    public void DestroyEntities()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemies)
        {
            Destroy(e);
        }

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
