using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawnStats : Stats
{

    private int maxHealth;

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        maxHealth = health;

        base.Start();
    }

    public int GetHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}
