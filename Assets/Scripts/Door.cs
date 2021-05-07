using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float unlockTime = 5;
    [SerializeField] private bool LeftDoor = false;
    [SerializeField] private float openDistance = 5;

    private float distToPlayer;
    private float timeUnlocked;
    private string animVar;
    private bool unlocked = false;

    private Transform player;

    private Animator anim;
    private BoxCollider2D col;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        if (unlockTime <= 0)
            unlockTime = 3;

        animVar = LeftDoor ? "OpenLeft" : "OpenRight";
    }

    private void Update()
    {
        //Get distance to player
        distToPlayer = Vector3.Distance(transform.position, player.position);

        //Has the door been unlocked
        if (unlocked)
        {
            //If the player is within range, open the door
            if (distToPlayer <= openDistance)
            {
                anim.SetBool(animVar, true);
                col.enabled = false;
            }
            //Otherwise if timeout or the door has been opened and the player moves out of range, close and lock the door again
            else if (Time.time >= timeUnlocked + unlockTime || anim.GetBool(animVar))
            {
                unlocked = false;
                col.enabled = true;
            }

        } else
        {
            //Door is closed at all times when door is locked
            anim.SetBool(animVar, false);
        }
    }

    public void UnlockDoor()
    {
        unlocked = true;

        timeUnlocked = Time.time;

    }
}
