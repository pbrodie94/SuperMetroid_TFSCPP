using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform destination;

    [SerializeField] private Transform doorPortL;
    [SerializeField] private Transform doorPortR;

    [SerializeField] private float unlockTime = 5;
    [SerializeField] private bool RightDoor = false;
    [SerializeField] private float openDistance = 5;

    private bool locked = false;
    [SerializeField] private bool missileLocked = false;

    private float distToPlayer;
    private float timeUnlocked;
    private string animVar;
    private bool unlocked = false;

    private Transform player;
    private SamusControl sc;
    private Room room;
    private HUDManager hud;
    private Animator anim;
    private BoxCollider2D col;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sc = player.gameObject.GetComponent<SamusControl>();
        room = transform.GetComponentInParent<Room>();
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        if (unlockTime <= 0)
            unlockTime = 3;

        animVar = RightDoor ? "OpenLeft" : "OpenRight";

        anim.SetBool("MissileLocked", missileLocked);
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
        if (!locked && !missileLocked)
        {
            unlocked = true;

            timeUnlocked = Time.time;
        }

    }

    public void DestroyMissileLock()
    {
        missileLocked = false;
        anim.SetBool("MissileLocked", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Room destinationRoom = destination.gameObject.GetComponentInParent<Room>();

            StartCoroutine(destinationRoom.TransitionToRoom(room, destination));
        }
    }

    public void SetMissileLock(bool mLock)
    {
        missileLocked = mLock;
        anim.SetBool("MissileLocked", mLock);
    }

    public void SetDoorLocked(bool locked)
    {
        this.locked = locked;
        anim.SetBool("Locked", locked);
    }

    public void SetDestination(Transform d)
    {
        destination = d;
    }

    public Transform GetDoorPort()
    {
        return RightDoor ? doorPortL : doorPortR;
    }
}
