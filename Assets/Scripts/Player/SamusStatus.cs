using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusStatus : MonoBehaviour
{
    private int energy = 99;
    private int energyTanks = 0;
    private int maxEnergyTanks = 0;

    HUDManager hud;

    private void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();

        hud.InitializeEnergy(energy, energyTanks, maxEnergyTanks);
    }

    public void TakeDamage(int damage)
    {
        energy -= damage;

        if (energy <= 0 && energyTanks > 0)
        {
            int excessDamage = Mathf.Abs(energy);

            energyTanks--;
            energy = 99;

            energy -= excessDamage;
        } else
        {
            Die();
        }

        hud.UpdateEnergy(energy, energyTanks, maxEnergyTanks);
    }

    public void PickupEnergyTank()
    {
        maxEnergyTanks++;
        energyTanks = maxEnergyTanks;
        energy = 99;

        hud.UpgradeEnergyTanks(energy, energyTanks, maxEnergyTanks);
    }

    public void PickupReserveTank()
    {
        energyTanks = maxEnergyTanks;
        energy = 99;

        hud.UpdateEnergy(energy, energyTanks, maxEnergyTanks);
    }

    public void PickupEnergy(int value)
    {
        energy += value;

        if (energy > 99)
        {
            if (maxEnergyTanks > 0 && energyTanks < maxEnergyTanks)
            {
                int excess = energy - 99;
                energyTanks++;
                energy = excess;
            } else
            {
                energy = 99;
            }
        }

        hud.UpdateEnergy(energy, energyTanks, maxEnergyTanks);
    }

    void Die()
    {
        //Dying animation, restart at checkpoint, or start
    }
}
