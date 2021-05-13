using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawn : MonoBehaviour
{
    [Header("Mechanics")]
    [SerializeField] private int moveSpeed = 40;
    private float moveDuration;
    [SerializeField] private Vector2 moveDurationRange;
    [SerializeField] private float percentIncrease = 0.2f; //max 20% increase
    private int maxHp = 500;

    [Header ("References")]
    [SerializeField] private Transform wallMount;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private Transform[] ropeBois;

    private SporeSpawnStats stats;

    private bool pausing = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        stats = GetComponentInChildren<SporeSpawnStats>();

        if (moveDurationRange == Vector2.zero)
        {
            moveDurationRange = new Vector2(5, 10);
        }

        if (moveSpeed <= 0 )
        {
            moveSpeed = 50;
        }

        moveDuration = Random.Range(moveDurationRange.x, moveDurationRange.y);

        maxHp = stats.GetHealth();
    }

    private void FixedUpdate()
    {
        UpdateRopePosition();
    }

    private IEnumerator OpenMouth(float waitTime)
    {
        //Stop moving 
        pausing = true;
        rb.velocity = Vector2.zero;

        //Open mouth

        yield return new WaitForSeconds(waitTime);

        //Close mouth

        yield return new WaitForSeconds(2);

        moveDuration = Random.Range(moveDurationRange.x, moveDurationRange.y);

        pausing = false;
    }

    void UpdateRopePosition()
    {
        //Places all 7 of the rope bois equally between the wall mount and the pivot

        //Get vector in direction of the line
        Vector3 dir = pivotPoint.position - wallMount.position;
        dir.Normalize();

        //Get distance
        float dist = Vector3.Distance(wallMount.position, pivotPoint.position);

        //Divide the distance into 8 (Staring position is wall mount and adds to the position, last point is the pivot point)
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

    private void UpdateMoveRange()
    {
        //Based on health decline
        int hp = stats.GetHealth();
        float pIncrease = 1 - (hp / maxHp); //Get health percentage from max, and subtract it from 1
        pIncrease = pIncrease <= 0.1f ? 0 : pIncrease; //If health is at less than 10%, max increase

        pIncrease = (percentIncrease * pIncrease) + 1; //Vary the increase by the range of health left, and give it a 1 so it adds

        moveSpeed += Mathf.RoundToInt(moveSpeed * pIncrease);
        
    }
}
