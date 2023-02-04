using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D body;

    public Transform pointA;
    public Transform pointB;

    public float speed;

    private bool goingToB = true;

    private Vector3 targetPos;


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

}
