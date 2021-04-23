using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public enum PickupType {Missile, Health}
    public PickupType pickup;
    [SerializeField] private int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (pickup)
            {
                case PickupType.Missile:

                    WeaponManager wm = collision.gameObject.GetComponent<WeaponManager>();
                    wm.PickupMissiles(value);

                    Destroy(gameObject);

                    break;

                case PickupType.Health:

                    Destroy(gameObject);

                    break;

                default:

                    Destroy(gameObject);

                    break;
            }
        }
    }
}
