using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    [Space]

    [SerializeField] private float groundCastDistance = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float fallMultiplier = 2f;

    private Collider2D collider2D;
    private Rigidbody2D body;
    private float groundedBuffer = 0;

    private float jumpBuffer = 0f;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x,y);

        Walk(dir);

        if(CastSelf(Vector2.down, groundCastDistance))
        {
            groundedBuffer = coyoteTime;
        }
        //On space set jump buffer
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBuffer = jumpBufferTime;
        }
        
        //If coyote time still ticking down and if jump buffer still ticking down, jump.
        if(groundedBuffer > 0f && jumpBuffer > 0f)
        {
            Jump();
        }

        if(body.velocity.y < 0f)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if(jumpBuffer > 0f) jumpBuffer -= Time.deltaTime;
        if(groundedBuffer > 0f) groundedBuffer -= Time.deltaTime;
    }

    void Walk(Vector2 dir)
    {
        body.velocity = new Vector2(dir.x * speed, body.velocity.y);
    }

    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    bool CastSelf(Vector3 direction, float distance)
    {
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(transform.position, collider2D.bounds.size, 0f, direction, distance, groundMask);

        return hit;
    }
}