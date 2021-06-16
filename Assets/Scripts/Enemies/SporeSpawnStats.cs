using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawnStats : Stats
{
    private int maxHealth;

    [Header("Audio")]
    [SerializeField] private AudioSource vocalAudio;

    [SerializeField] private AudioClip[] hurtAudio;
    [SerializeField] private AudioClip dieAudio;

    private float hurtAudioInterval = 1;
    private float timeLastHurtAudio = 0;
    
    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        maxHealth = health;

        base.Start();
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
        }
        else if (vocalAudio && hurtAudio[0])
        {

            if (Time.time >= timeLastHurtAudio + hurtAudioInterval)
            {
                int hurtIndex = 0;

                if (hurtAudio.Length > 1)
                {
                    hurtIndex = Random.Range(0, hurtAudio.Length - 1);
                }

                vocalAudio.PlayOneShot(hurtAudio[hurtIndex]);

                timeLastHurtAudio = Time.time;
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    private void Die()
    {
        if (vocalAudio && dieAudio)
        {
            vocalAudio.PlayOneShot(dieAudio);
        }
    }
}
