using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeDude : MonoBehaviour
{
    [SerializeField] private GameObject spore;
    [SerializeField] private Transform mouth;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator Shoot()
    {
        anim.SetBool("Open", true);

        yield return new WaitForSeconds(1.5f);

        Instantiate(spore, mouth.position, Quaternion.identity);

        yield return new WaitForSeconds(1);

        anim.SetBool("Open", false);
    }
}
