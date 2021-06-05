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
    [SerializeField] private int missiles = 0;
    [SerializeField] private int maxMissiles = 0;

    [Header("Weapon Stats")]
    [SerializeField] private float projectileVelocity = 1000;
    [Tooltip("Fire Rate in bullets per second")]
    [SerializeField] private float fireRate = 300;
    private float timeLastShot = 0;

    [Header("Powerbeam Stats")]
    [SerializeField] GameObject powerBeamProjectile;
    [SerializeField] private int powerBeamDamage = 10;
    
    [Header("Missile Stats")]
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private int missileDamage = 20;
    

    [Header("Barrel Locations")]
    [SerializeField] private Transform[] barrelPositions;

    [Header("Audio")]
    [SerializeField] private AudioSource weaponAudio;
    [SerializeField] private AudioClip powerBeamShot;
    [SerializeField] private AudioClip missileShot;

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
        if (hud.IsPaused())
            return;

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
        int damage = powerBeamDamage;
        AudioClip shot = powerBeamShot;

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
                damage = powerBeamDamage;
                shot = powerBeamShot;

                break;

            case WeaponType.Missile:

                proj = missileProjectile;
                damage = missileDamage;
                shot = missileShot;

                break;

            default:

                proj = powerBeamProjectile;
                damage = powerBeamDamage;

                break;
        }

        GameObject go = Instantiate(proj, barrel, Quaternion.identity);
        go.gameObject.GetComponent<Projectile>().SetStats(gameObject, damage);
        Rigidbody2D b = go.GetComponent<Rigidbody2D>();
        weaponAudio.PlayOneShot(shot);

        if (weapon == WeaponType.Missile)
        {
            Animator a = go.GetComponent<Animator>();
            a.SetInteger("Direction", projectileDirection);

            missiles--;

            hud.UpdateMissiles(missiles);
        }

        //b.gravityScale = 0;
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

    public void IncreaseMissiles(int amount)
    {
        maxMissiles += amount;
        missiles = maxMissiles;

        hud.UpdateMissiles(missiles);
    }

    public int GetMissileMax()
    {
        return maxMissiles;
    }
}

