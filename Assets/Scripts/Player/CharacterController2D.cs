using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{

    //Private
    [SerializeField] private bool airControl = true;
    [Range(0, 1)] [SerializeField] private float movementSmoothing = 0.5f;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] private float goundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    public float jump { set { jumpForce = value; } }
    private float jumpForce = 400;
    private bool facingRight = true;
    public bool TwoWayAnims { set { twoWayAnims = value; } }
    private bool twoWayAnims = true;
    private Vector3 smoothingVelocity = Vector3.zero;

    //Components
    private SpriteRenderer characterRenderer;
    private Animator anim;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = transform.GetComponentInChildren<Animator>();
        characterRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    public void Move(float move, bool jump)
    {
        if (IsGrounded() || airControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref smoothingVelocity, movementSmoothing);

            if (move > 0 && !facingRight)
            {
                Flip();
            }

            if (move < 0 && facingRight)
            {
                Flip();
            }

            anim.SetFloat(AnimationVars.Speed, Mathf.Abs(move));

        }

        //Debug.Log(IsGrounded());

        if (jump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }

    public bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPosition.position, goundCheckRadius, groundMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    private void Flip()
    { 
        facingRight = !facingRight;

        if (twoWayAnims)
        {
            anim.SetBool(AnimationVars.FacingRight, facingRight);
        } else
        {
            characterRenderer.flipX = !characterRenderer.flipX;
        }
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
