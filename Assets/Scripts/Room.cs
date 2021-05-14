using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("Takes in a reference of the door and its destination, and the sets it to the door.")]
    [SerializeField] private DoorDestinations[] doorDestinations;

    [Tooltip("Sets the X axis bounds for camera movement")]
    [SerializeField] private Vector2 xAxisBounds = Vector2.zero;
    [Tooltip("Sets the y axis bounds for camera movement")]
    [SerializeField] private Vector2 yAxisBounds = Vector2.zero;

    [SerializeField] private EnemySpawnPoint[] spawnPoints;

    private Transform player;

    private CameraFollow2D cam;
    private SamusControl sc;
    private HUDManager hud;
    private GameManager gm;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        cam = Camera.main.GetComponent<CameraFollow2D>();
        sc = player.gameObject.GetComponent<SamusControl>();
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (doorDestinations.Length > 0)
        {
            foreach (DoorDestinations d in doorDestinations)
            {
                d.SetDestination();
            }
        }
    }

    public virtual void SetUpRoom()
    {
        SetCameraBounds();
        SpawnEntities();
    }

    protected void SetCameraBounds()
    {
        cam.SetCameraBounds(xAxisBounds, yAxisBounds);
    }

    protected void SpawnEntities()
    {
        if (spawnPoints.Length > 0)
        {
            foreach (EnemySpawnPoint sp in spawnPoints)
            {
                sp.SpawnEnemy();
            }
        }
    }

    public IEnumerator TransitionToRoom(Room previousRoom, Transform destination)
    {
        sc.SetControl = false;

        //Fade screen to black
        hud.FadeScreen(true);

        yield return new WaitForSeconds(0.5f);

        //Destroy enemies and pickups in current room
        previousRoom.DestroyEntities();

        yield return null;

        //Move player to new room
        player.position = destination.position;
        Vector3 pos = player.position;
        pos.z = cam.transform.position.z;
        cam.transform.position = pos;

        gm.SetCurrentRoom(this);

        yield return null;

        //Spawn enemies and pickups in new room
        SetUpRoom();

        yield return new WaitForSeconds(0.5f);

        //Fade screen back
        hud.FadeScreen(false);

        yield return new WaitForSeconds(1.5f);

        sc.SetControl = true;
    }

    public void SetSpawnRoom(Room previousRoom, Transform destination)
    {
        //Destroy enemies and pickups in current room
        previousRoom.DestroyEntities();

        //Move player to new room
        player.position = destination.position;
        Vector3 pos = player.position;
        pos.z = cam.transform.position.z;
        cam.transform.position = pos;

        gm.SetCurrentRoom(this);

        SetUpRoom();
    }

    public virtual void DestroyEntities()
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

[System.Serializable]
public struct DoorDestinations
{
    public Door door;
    public Door destinationDoor;

    public void SetDestination()
    {
        door.SetDestination(destinationDoor.GetDoorPort());
    }
}
