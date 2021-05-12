using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawn : MonoBehaviour
{
    [SerializeField] private Transform wallMount;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private Transform[] ropeBois;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateRopePosition();
    }

    void UpdateRopePosition()
    {
        //Places all 7 of the rope bois equally between the wall mount and the pivot

        //Get vector in direction of the line
        Vector3 dir = pivotPoint.position - wallMount.position;
        dir.Normalize();

        //Get distance
        float dist = Vector3.Distance(wallMount.position, pivotPoint.position);

        //Divide the distance into 9 (the 8th and 9th position is the wall mount and pivot)
        float interval = dist / 8;

        //Place all the rope bois 
        for (int i = 0; i < ropeBois.Length; i++)
        {
            //Get the point along the line and multiply the magnitude
            Vector3 pos = dir * ((i + 1) * interval);
            pos.z = 0;

            ropeBois[i].position = wallMount.position + pos;

        }
    }
}
