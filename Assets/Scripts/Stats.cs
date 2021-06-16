using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] protected int health;

    [SerializeField] protected float flashInterval = 0.1f;
    [SerializeField] protected float flashDuration = 0.2f;

    protected bool dead = false;

    protected SpriteRenderer spriteRenderer;
    protected Material defaultMaterial;
    protected Material flashMaterial;

    protected virtual void Start()
    {
        
        flashMaterial = Resources.Load("FlashMaterial", typeof(Material)) as Material;

        if (flashInterval <= 0)
        {
            flashInterval = 0.1f;
        }

        if (flashDuration <= 0)
        {
            flashDuration = 0.2f;
        }

        dead = false;
    }

    public virtual void TakeDamage(int damage)
    {
        if (dead)
            return;

        health -= damage;

        StartCoroutine(Flash(flashInterval, flashDuration, Time.time));
    }

    protected virtual IEnumerator Flash(float interval, float duration, float flashStart)
    {
        do
        {
            spriteRenderer.material = flashMaterial;

            yield return new WaitForSeconds(interval);

            spriteRenderer.material = defaultMaterial;

            yield return new WaitForSeconds(interval);

        } while (Time.time < flashStart + duration);
    }
}
