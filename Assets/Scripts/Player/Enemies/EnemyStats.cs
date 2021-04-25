using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{

    private Animator anim;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        anim = GetComponent<Animator>();
    }

    protected override void Die()
    {
        base.Die();

        anim.SetBool("Dead", true);

        Destroy(gameObject, 1);
    }
}
