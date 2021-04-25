using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZebesian : MonoBehaviour
{

    [SerializeField] private float detectionRadius;

    private float distToPlayer;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.position);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
