using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawn : MonoBehaviour
{
    private int phase = 1;
    private bool battling = false;

    private int damage;
    [SerializeField] private int baseDamage = 60;
    private float rushDamagheMultiplier = 1.35f;

    [Header("Mechanics")]
    [Range(1, 3)]
    [SerializeField] private float moveSpeedIncrease = 1;
    [SerializeField] private float movementSmoothing = 0.5f;
    [SerializeField] private Vector2 baseMoveAmplitude;
    private Vector2 moveAmplitude;
    [SerializeField] private Vector2 actionDurationRange;
    [SerializeField] private float percentIncrease = 0.2f; //max 20% increase
    private float moveTimer = 0;

    private Vector3 wayPoint = Vector3.zero;
    private bool waypointPhase = false;
    private float waypointMoveSpeed = 10;
    private int numMoves = 0;
    private int moves = 0;

    [Header ("References")]
    [SerializeField] private Transform wallMount;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private Transform core;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Transform[] ropeBois;

    private int maxHp;
    private float actionDuration;
    private float timeStartAction;
    private bool moving = false;
    private bool pausing = false;
    private bool resetPosition = true;
    private Vector3 defaultPosition = Vector3.zero;
    private Vector3 startPosition = new Vector3(148.02f, -71, 0);
    private Vector3 moveVelocity = Vector3.zero;

    private SporeSpawnStats stats;

    private Rigidbody2D rb;
    private PolygonCollider2D col;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<PolygonCollider2D>();

        if (!core)
            core = transform.Find("Core");

        core.tag = "Untagged";

        stats = GetComponentInChildren<SporeSpawnStats>();

        if (actionDurationRange == Vector2.zero)
        {
            actionDurationRange = new Vector2(5, 10);
        }

        if (baseDamage <= 0)
        {
            baseDamage = 60;
        }

        defaultPosition = transform.position;

        maxHp = stats.GetHealth();
        waypointMoveSpeed = 10;
        moveAmplitude = baseMoveAmplitude;
        damage = baseDamage;
    }

    private void FixedUpdate()
    {
        if (battling)
        {
            if (resetPosition)
            {
                float dist = Vector3.Distance(transform.position, startPosition);

                if (dist > 0.1f)
                {
                    Vector3 dir = startPosition - transform.position;

                    rb.MovePosition(transform.position + dir.normalized * 5 * Time.fixedDeltaTime);

                    UpdateRopePosition();

                }
                else
                {
                    actionDuration = Random.Range(actionDurationRange.x, actionDurationRange.y);
                    timeStartAction = Time.time;
                    moveTimer = 0;
                    moving = true;
                    resetPosition = false;
                }
            }

            if (moving)
            {
                if (!waypointPhase && Time.time > timeStartAction + actionDuration)
                {

                    rb.velocity = Vector2.zero;

                    if (phase > 1)
                    {
                        damage = Mathf.RoundToInt(damage * rushDamagheMultiplier);
                        waypointPhase = true;
                        numMoves = Random.Range(2, 8);
                        wayPoint = GetNewWaypoint(wayPoint);
                        timeStartAction = Time.time;
                    }
                    else
                    {

                        moving = false;
                    }
                } else if (waypointPhase)
                {

                    if (Time.time >= timeStartAction + 0.5f)
                    {
                        float distToPoint = Vector3.Distance(transform.position, wayPoint);

                        if (distToPoint <= 0.2f)
                        {
                            moves++;

                            if (moves >= numMoves)
                            {
                                moves = 0;
                                waypointPhase = false;
                                moving = false;

                                damage = baseDamage;
                            }

                            wayPoint = GetNewWaypoint(wayPoint);
                            timeStartAction = Time.time;
                        }

                        Vector3 dir = wayPoint - transform.position;

                        rb.MovePosition(transform.position + dir.normalized * waypointMoveSpeed * Time.fixedDeltaTime);
                    } else
                    {
                        rb.velocity = Vector2.zero;
                    }

                } else
                {
                    FigureEight();
                }

                UpdateRopePosition();

            }
            else
            {
                if (!pausing && !resetPosition && !waypointPhase)
                {
                    float wait = Random.Range(3, 5);
                    StartCoroutine(OpenMouth(wait));
                }

                
            }
        }
    }

    void FigureEight()
    {
        moveTimer += Time.fixedDeltaTime;

        float x = Mathf.Cos(moveSpeedIncrease * moveTimer) * (moveAmplitude.x * moveSpeedIncrease) * Time.deltaTime * 40;
        float y = Mathf.Cos(moveSpeedIncrease * (2 * moveTimer)) * (moveAmplitude.y * moveSpeedIncrease) * Time.deltaTime * 40;

        Vector3 targetVelocity = new Vector3(x, y, 0);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref moveVelocity, movementSmoothing);
    }

    Vector3 GetNewWaypoint(Vector3 currentWaypoint)
    {
        Vector3 wp = Vector3.zero;

        do
        {
            int rand = Random.Range(0, wayPoints.Length - 1);

            wp = wayPoints[rand].position;

        } while (wp == currentWaypoint);

        return wp;
    }

    private IEnumerator OpenMouth(float waitTime)
    {
        //Stop moving 
        pausing = true;
        col.enabled = false;
        rb.velocity = Vector2.zero;

        //Open mouth
        anim.SetBool("OpenMouth", true);

        yield return new WaitForSeconds(waitTime);

        //Close mouth
        anim.SetBool("OpenMouth", false);

        yield return new WaitForSeconds(1);

        UpdatePhase();

        UpdateMoveRange();

        resetPosition = true;
        col.enabled = true;
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

    private void UpdatePhase()
    {
        //Get current health
        int hp = stats.GetHealth();
        //Get the next phase threshold
        int phaseThreshold = maxHp - (phase * (maxHp / 3));

        if (hp <= phaseThreshold)
        {
            phase++;
        }
    }

    private void UpdateMoveRange()
    {
        //Based on health decline
        int hp = stats.GetHealth();
        float pChange = 1f - ((float)hp / (float)maxHp); //Get health percentage from max, and subtract it from 1
        Debug.Log("HP: " + hp + "maxHp: " + maxHp + " Pchange: " + pChange);
        pChange = pChange > 0.9f ? 1 : pChange; //If health is at less than 10%, max increase

        moveSpeedIncrease = 1 + (2 * pChange);

        moveAmplitude.x = baseMoveAmplitude.x + ((10 - baseMoveAmplitude.x) * pChange);
        moveAmplitude.y = baseMoveAmplitude.y + ((10 - baseMoveAmplitude.y) * pChange);

        pChange = 1 + (percentIncrease * pChange);
        actionDurationRange = new Vector2(actionDurationRange.x * pChange, actionDurationRange.y * pChange);

        waypointMoveSpeed = waypointMoveSpeed + ((20 - waypointMoveSpeed) * pChange);
    }

    public IEnumerator BeginBattle()
    {
        core.tag = "Enemy";

        yield return new WaitForSeconds(2);

        timeStartAction = Time.time;
        actionDuration = Random.Range(actionDurationRange.x, actionDurationRange.y);

        battling = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Stats s = collision.gameObject.GetComponent<Stats>();
            s.TakeDamage(damage);
        }    
    }

    public void ReportPlayerCollision(GameObject p)
    {
        if (pausing)
        {
            Stats s = p.GetComponent<Stats>();
            s.TakeDamage(damage);
        }
    }

    public void ResetBoss()
    {
        core.tag = "Untagged";

        stats.ResetHealth();

        transform.position = defaultPosition;
        UpdateRopePosition();
    }
}