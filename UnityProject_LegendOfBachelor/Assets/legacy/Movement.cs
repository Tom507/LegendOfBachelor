using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 input = Vector3.zero;

    public float maxSpeed = 5f;
    public float acccelleration = 10;
    public float accellFactor = 2;

    public float drag = 5;

    public float velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity.magnitude;

        acccelleration = maxSpeed * accellFactor;

        rb.AddForce(input * acccelleration);

        //Drag
        if (input.magnitude < 0.1 && rb.velocity.magnitude > 0f)
            rb.AddForce( - rb.velocity * drag );

        //capping velocity
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

}
