using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private float lifeTime = 10;
    public enum PickupType {Missile, Health, MissileBoost, ReserveTank, EnergyTank, LifeUp}
    public PickupType pickup;
    [SerializeField] private int value;

    private void Start()
    {
        if (lifeTime > 0)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    public void SetLifetime(float lifeTime)
    {
        if (lifeTime > 0)
        {
            this.lifeTime = lifeTime;

            Destroy(gameObject, lifeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            WeaponManager wm = collision.gameObject.GetComponent<WeaponManager>();
            SamusStatus stats = collision.gameObject.GetComponent<SamusStatus>();

            switch (pickup)
            {
                case PickupType.Missile:
                    
                    wm.PickupMissiles(value);
                    Destroy(gameObject);

                    break;

                case PickupType.Health:
                    //Add health

                    stats.PickupEnergy(value);
                    Destroy(gameObject);

                    break;

                case PickupType.MissileBoost:
                    //Increase max missiles by 5

                    wm.IncreaseMissiles(value);
                    Destroy(gameObject);

                    break;

                case PickupType.ReserveTank:
                    //Max health
                    stats.PickupReserveTank();
                    Destroy(gameObject);

                    break;

                case PickupType.EnergyTank:
                    //Increase energy tanks by 1

                    stats.PickupEnergyTank();
                    Destroy(gameObject);

                    break;

                case PickupType.LifeUp:
                    //Add lives

                    stats.PickupLives();
                    Destroy(gameObject);
                    break;

                default:

                    Destroy(gameObject);

                    break;
            }
        }
    }
}
