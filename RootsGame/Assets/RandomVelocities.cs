using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVelocities : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.velocity = Random.insideUnitCircle;
        body.angularVelocity = Random.Range(-180, 180);

        Destroy(this.gameObject, 2f);
    }

   
}
