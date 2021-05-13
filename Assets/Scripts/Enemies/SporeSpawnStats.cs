using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawnStats : Stats
{

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Start();
    }

    public int GetHealth()
    {
        return health;
    }
}
