using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D body;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(body.velocity.x > 0f && sr.flipX)
        {
            sr.flipX = false;
        }

        if(body.velocity.x < 0f && !sr.flipX)
        {
            sr.flipX = true;
        }

        if(Mathf.Abs(body.velocity.x) > 0.01f)
        {
            SetWalking(true);
        }
        else
        {
            SetWalking(false);
        }
    }

    public void SetWalking(bool state)
    {
        animator.SetBool("isWalking", state);
    }

    public void JumpTrigger()
    {
        animator.SetTrigger("jumpTrigger");
    }

    public void LandTrigger()
    {
        animator.SetTrigger("landTrigger");
    }
}
