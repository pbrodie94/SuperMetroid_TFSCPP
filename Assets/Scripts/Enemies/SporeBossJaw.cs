using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeBossJaw : MonoBehaviour
{

    private SporeSpawn boss;

    private void Start()
    {
        boss = GetComponentInParent<SporeSpawn>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Tell boss boi
            boss.ReportPlayerCollision(collision.gameObject);
        }
    }
}
