using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusStatus : Stats
{
    private int energyTanks = 0;
    private int maxEnergyTanks = 0;

    HUDManager hud;

    protected override void Start()
    {
        base.Start();

        hud = GameObject.Find("HUD").GetComponent<HUDManager>();

        hud.InitializeEnergy(health, energyTanks, maxEnergyTanks);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        if (health <= 0)
        {
            health = 99;
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(Flash(flashInterval, flashDuration, Time.time));

        if (health <= 0 && energyTanks > 0)
        {
            int excessDamage = Mathf.Abs(health);

            energyTanks--;
            health = 99;

            health -= excessDamage;
        } else
        {
            Die();
        }

        hud.UpdateEnergy(health, energyTanks, maxEnergyTanks);
    }

    public void PickupEnergyTank()
    {
        maxEnergyTanks++;
        energyTanks = maxEnergyTanks;
        health = 99;

        hud.UpgradeEnergyTanks(health, energyTanks, maxEnergyTanks);
    }

    public void PickupReserveTank()
    {
        energyTanks = maxEnergyTanks;
        health = 99;

        hud.UpdateEnergy(health, energyTanks, maxEnergyTanks);
    }

    public void PickupEnergy(int value)
    {
        health += value;

        if (health > 99)
        {
            if (maxEnergyTanks > 0 && energyTanks < maxEnergyTanks)
            {
                int excess = health - 99;
                energyTanks++;
                health = excess;
            } else
            {
                health = 99;
            }
        }

        hud.UpdateEnergy(health, energyTanks, maxEnergyTanks);
    }

    protected override void Die()
    {
        //Dying animation, restart at checkpoint, or start
    }
}
