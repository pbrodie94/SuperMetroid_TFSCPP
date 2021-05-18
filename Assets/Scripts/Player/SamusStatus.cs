using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusStatus : Stats
{
    private int energyTanks = 0;
    private int maxEnergyTanks = 0;

    [SerializeField] private int lives = 3;
    private bool dead = false;
    public bool isDead 
    { 
        get
        {
            return dead;
        } 
    }

    SpriteRenderer renderer;
    private string DeathLayer = "DeathLayer";
    private string PlayLayer = "MidGround";
    Animator anim;

    [Header("Audio")]
    [SerializeField] private AudioSource voiceAudio;
    [SerializeField] private AudioClip[] hurtAudio;
    [SerializeField] private AudioClip[] lowHealthPant;
    [SerializeField] private AudioClip dieAudio;

    HUDManager hud;
    GameManager gm;

    protected override void Start()
    {
        base.Start();

        renderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        hud.InitializeEnergy(health, energyTanks, maxEnergyTanks);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        if (health <= 0)
        {
            health = 99;
        }

        if (lives <= 0)
        {
            lives = 3;
        }

        hud.UpdateLives(lives);
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;

        int hurtIndex = Random.Range(0, hurtAudio.Length);
        voiceAudio.PlayOneShot(hurtAudio[hurtIndex]);

        StartCoroutine(Flash(flashInterval, flashDuration, Time.time));

        if (health <= 0 && energyTanks > 0)
        {
            int excessDamage = Mathf.Abs(health);

            energyTanks--;
            health = 99;

            health -= excessDamage;
        } else if (health <= 0 && energyTanks <= 0 && !dead)
        {
            health = 0;
            hud.UpdateEnergy(health, energyTanks, maxEnergyTanks);
            dead = true;

            StartCoroutine(Die());
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

    public void PickupLives()
    {
        lives++;
        hud.UpdateLives(lives);
    }

    public void PickupEnergy(int value)
    {
        health += value;

        //Manage energy tanks if health is greater than 99
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

    private IEnumerator Die()
    {
        //Dying animation
        renderer.sortingLayerName = DeathLayer;
        gm.PlayerDie();
        hud.FadeUI(true);
        anim.SetBool("Dead", true);

        yield return new WaitForSeconds(8);

        //restart at checkpoint, or start
        if (lives > 0)
        {
            lives--;
            hud.UpdateLives(lives);
            PickupReserveTank();

            //Respawn Player
            gm.RespawnPlayer();
            renderer.sortingLayerName = PlayLayer;

            yield return null;

            anim.SetBool("Dead", false);
            anim.SetTrigger("Respawn");

            yield return new WaitForSeconds(2);

            hud.FadeUI(false);

            yield return new WaitForSeconds(5);

            dead = false;

            yield return null;

            gm.PlayerSpawned();
        }
        else
        {
            //Game over
        }
    }
}
