using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 40;
    [SerializeField] private float jumpHeight = 400;
    private float horizontalInput = 0;

    [Header("Combat")]
    

    private bool jump = false;

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
        horizontalInput = Input.GetAxisRaw(InputManager.horizontal) * moveSpeed;

        if (Input.GetButtonDown(InputManager.jump))
        {
            jump = true;
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
