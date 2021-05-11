using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    private EnemyAI ai;

    private Animator anim;

    protected override void Start()
    {
        base.Start();

        ai = GetComponent<EnemyAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        anim = GetComponent<Animator>();

        GameManager.OnPlayerSpawn += ai.GetPlayerLocation;
        GameManager.OnPlayerSpawn += ai.PlayerSpawned;
        GameManager.OnPlayerDeath += ai.PlayerDeath;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        anim.SetBool("Dead", true);

        GameManager.OnPlayerSpawn -= ai.GetPlayerLocation;
        GameManager.OnPlayerSpawn -= ai.PlayerSpawned;
        GameManager.OnPlayerDeath -= ai.PlayerDeath;

        Destroy(gameObject, 1);
    }
}
