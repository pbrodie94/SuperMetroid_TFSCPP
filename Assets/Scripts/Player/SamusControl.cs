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
    private bool doubleJump = true;
    private bool hasDoubleJumped = false;
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

        if (Input.GetButtonDown(InputManager.jump) && (controller.IsGrounded() || (doubleJump && !hasDoubleJumped)))
        {
            jump = true;
            suitAudio.PlayOneShot(jumpAudio);
            
            if (controller.IsGrounded())
            {
                anim.SetBool(AnimationVars.Jumping, jump);
            } else
            {
                hasDoubleJumped = true;
                anim.SetBool(AnimationVars.DoubleJumping, true);
            }
        }

        if (Input.GetButtonUp(InputManager.fire))
        {
            anim.SetBool(AnimationVars.Attacking, false);
        }

        if (controller.IsGrounded())
        {
            hasDoubleJumped = false;
            anim.SetBool(AnimationVars.Jumping, false);
            anim.SetBool(AnimationVars.DoubleJumping, false);

        } else
        {
            if (anim.GetBool(AnimationVars.Jumping))
            {
                if (controller.GetVelocity().y < 0)
                {
                    //Falling
                    anim.SetBool(AnimationVars.Jumping, false);
                }

            }
        }

        anim.SetBool(AnimationVars.Grounded, controller.IsGrounded());

        Debug.Log("Velocity: " + controller.GetVelocity() + " Animator Jumping: " + anim.GetBool(AnimationVars.Jumping));
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalInput * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
