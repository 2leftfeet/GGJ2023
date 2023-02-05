using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Rootable
{
    private Rigidbody2D body;

    public Transform pointA;
    public Transform pointB;

    public float speed;

    private bool goingToB = true;

    private Vector3 targetPos;
    private bool isRooted = false;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        targetPos = pointB.position;
        body.velocity = (targetPos - transform.position).normalized * speed;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            goingToB = !goingToB;
            targetPos = goingToB ? pointB.position : pointA.position;

            body.velocity = (targetPos - transform.position).normalized * speed;
        }
    }

    public override void RootInPlace()
    {
        isRooted = true;
        body.velocity = Vector2.zero;
    }

    public override Collider2D GetCollider2D()
    {
        return GetComponent<Collider2D>();
    }

    public override bool IsRooted()
    {
        return isRooted;
    }

    public override void Unroot()
    {
        body.velocity = (targetPos - transform.position).normalized * speed;
        isRooted = false;

        if(rootEffect)
            rootEffect.ResetEffect();
        
    }

}
