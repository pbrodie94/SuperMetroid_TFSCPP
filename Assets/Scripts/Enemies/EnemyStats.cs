using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    private EnemyAI ai;

    private Animator anim;

    [Header("Audio")]
    [SerializeField] private AudioSource vocalAudio;

    [SerializeField] private AudioClip[] hurtAudio;
    [SerializeField] private AudioClip dieAudio;

    float hurtSoundInterval = 1;
    float timeLastHurtSound = 0;

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
        if (dead)
            return;

        base.TakeDamage(damage);

        if (health <= 0)
        {
            dead = true;

            Die();
        } else if (vocalAudio && hurtAudio[0])
        {
            Debug.Log("Can play sound");

            if (Time.time >= timeLastHurtSound + hurtSoundInterval)
            {
                Debug.Log("Should be playing sound");

                int hurtIndex = 0;

                if (hurtAudio.Length > 1)
                {
                    hurtIndex = Random.Range(0, hurtAudio.Length - 1);
                }

                vocalAudio.PlayOneShot(hurtAudio[hurtIndex]);

                timeLastHurtSound = Time.time;
            }
        }
    }

    protected void Die()
    {
        if (vocalAudio && dieAudio)
        {
            vocalAudio.PlayOneShot(dieAudio);
        }

        anim.SetBool("Dead", true);

        GameManager.OnPlayerSpawn -= ai.GetPlayerLocation;
        GameManager.OnPlayerSpawn -= ai.PlayerSpawned;
        GameManager.OnPlayerDeath -= ai.PlayerDeath;

        Destroy(gameObject, 1);
    }
}
