using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    public void SpawnEnemy()
    {
        int e = 0;

        if (enemies.Length > 1)
        {
            e = Random.Range(0, enemies.Length);
        }

        GameObject go = Instantiate(enemies[e], transform.position, Quaternion.identity);
        EnemyAI ai = go.GetComponent<EnemyAI>();
        ai.Initialize();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
