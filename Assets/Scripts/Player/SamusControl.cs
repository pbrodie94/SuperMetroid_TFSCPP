using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusControl : MonoBehaviour
{
    private bool control = true;
    [HideInInspector] public bool SetControl
    {
        set
        {
            control = value;
        }
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 40;
    [SerializeField] private float jumpHeight = 400;
    private float horizontalInput = 0;

    private bool jump = false;

    [Header("Audio")]
    [SerializeField] private AudioSource suitAudio;
    [SerializeField] private AudioClip jumpAudio;

    //Components
    private CharacterController2D controller;
    private Animator anim;

    private void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController2D>();

        controller.jump = jumpHeight;
        controller.TwoWayAnims = true;
    }

    private void Update()
    {
        if (!control)
        {
            horizontalInput = 0;
            jump = false;
            return;
        }

        horizontalInput = Input.GetAxisRaw(InputManager.horizontal) * moveSpeed;

        if (Input.GetButtonDown(InputManager.jump))
        {
            jump = true;
            suitAudio.PlayOneShot(jumpAudio);
        }

        if (Input.GetButtonUp(InputManager.fire))
        {
            anim.SetBool(AnimationVars.Attacking, false);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalInput * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
