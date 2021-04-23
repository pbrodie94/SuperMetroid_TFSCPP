using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 40;
    [SerializeField] private float jumpHeight = 400;
    private float horizontalInput = 0;

    [Header("Combat")]
    [SerializeField] GameObject powerBeamProjectile;
    [SerializeField] private float powerBeamDamage = 10;
    [SerializeField] private float powerBeamVelocity = 1000;
    [Tooltip("Fire Rate in bullets per second")]
    [SerializeField] private float fireRate = 120;
    private float timeLastShot = 0;

    [Header("Barrel Locations")]
    [SerializeField] private Transform midRightBarrel;
    [SerializeField] private Transform midLefttBarrel;

    private bool jump = false;

    //Components
    private CharacterController2D controller;
    private Animator anim;

    private void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController2D>();

        controller.jump = jumpHeight;
        controller.TwoWayAnims = true;

        fireRate = 60 / fireRate;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw(InputManager.horizontal) * moveSpeed;

        if (Input.GetButtonDown(InputManager.jump))
        {
            jump = true;
        }

        if (Input.GetButton(InputManager.fire))
        {
            if (Time.time >= (timeLastShot + fireRate))
            {
                Shoot();
            }

            anim.SetBool(AnimationVars.Attacking, true);
        }

        if (Input.GetButtonUp(InputManager.fire))
        {
            anim.SetBool(AnimationVars.Attacking, false);
        }
    }

    private void Shoot()
    {
        timeLastShot = Time.time;

        Transform barrel;
        float power;

        if (controller.IsFacingRight())
        {
            barrel = midRightBarrel;
            power = powerBeamVelocity;
        } else
        {
            barrel = midLefttBarrel;
            power = -powerBeamVelocity;
        }

        GameObject go = Instantiate(powerBeamProjectile, barrel.position, Quaternion.identity);
        Rigidbody2D b = go.GetComponent<Rigidbody2D>();

        b.gravityScale = 0;
        b.AddForce(new Vector2(power, 0));

        Destroy(go, 60);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalInput * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
