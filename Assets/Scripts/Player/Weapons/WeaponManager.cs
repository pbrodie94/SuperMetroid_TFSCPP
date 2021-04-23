using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private enum WeaponType
    {
        PowerBeam,
        Missile
    }

    [Header("Inventory")]
    [SerializeField] private int missiles = 5;
    [SerializeField] private int maxMissiles = 99;

    [Header("Weapon Stats")]
    [SerializeField] private float projectileVelocity = 1000;
    [Tooltip("Fire Rate in bullets per second")]
    [SerializeField] private float fireRate = 300;
    private float timeLastShot = 0;

    [Header("Powerbeam Stats")]
    [SerializeField] GameObject powerBeamProjectile;
    [SerializeField] private float powerBeamDamage = 10;
    
    [Header("Missile Stats")]
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private float missileDamage = 20;
    

    [Header("Barrel Locations")]
    [SerializeField] private Transform[] barrelPositions;

    HUDManager hud;

    CharacterController2D controller;
    Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        anim = transform.GetComponentInChildren<Animator>();

        hud = GameObject.Find("HUD").GetComponent<HUDManager>();

        hud.UpdateMissiles(missiles);

        fireRate = 60 / fireRate;
    }

    private void Update()
    {
        if (Input.GetButton(InputManager.fire))
        {
            if (Time.time >= (timeLastShot + fireRate))
            {
                Shoot(WeaponType.PowerBeam);
            }

            anim.SetBool(AnimationVars.Attacking, true);
        }

        if (Input.GetButtonDown(InputManager.missile))
        {
            //Fire Missile
            if (Time.time >= (timeLastShot + fireRate) && missiles > 0)
            {
                Shoot(WeaponType.Missile);
            }
        }
    }

    private void Shoot(WeaponType weapon)
    {
        timeLastShot = Time.time;

        GameObject proj = powerBeamProjectile;
        Vector3 barrel;
        Vector2 dir;
        int projectileDirection = 0;

        if (controller.IsFacingRight())
        {
            barrel = barrelPositions[0].position;
            dir = new Vector2(projectileVelocity, 0);

            projectileDirection = 0;
        }
        else
        {
            barrel = barrelPositions[1].position;
            dir = new Vector2(-projectileVelocity, 0);

            projectileDirection = 1;
        }

        switch (weapon)
        {
            case WeaponType.PowerBeam:

                proj = powerBeamProjectile;

                break;

            case WeaponType.Missile:

                proj = missileProjectile;

                break;

            default:

                proj = powerBeamProjectile;

                break;
        }

        GameObject go = Instantiate(proj, barrel, Quaternion.identity);
        Rigidbody2D b = go.GetComponent<Rigidbody2D>();

        if (weapon == WeaponType.Missile)
        {
            Animator a = go.GetComponent<Animator>();
            a.SetInteger("Direction", projectileDirection);

            missiles--;

            hud.UpdateMissiles(missiles);
        }

        b.gravityScale = 0;
        b.AddForce(dir);

        Destroy(go, 60);
    }

    public void PickupMissiles(int amount)
    {
        if (missiles < maxMissiles)
        {
            missiles += amount;

            if (missiles > maxMissiles)
            {
                missiles = maxMissiles;
            }
        }

        hud.UpdateMissiles(missiles);
    }
}

