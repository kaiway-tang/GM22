using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobileEntity : HPEntity
{
    [SerializeField] protected Rigidbody rb;
    Vector3 vect3;

    
    protected new void Start()
    {
        base.Start();
        trfm = transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    protected void SetXVel(float spd)
    {
        vect3.y = rb.velocity.y;
        vect3.z = rb.velocity.z;

        vect3.x = spd;
        rb.velocity = vect3;
    }
    protected void SetYVel(float spd)
    {
        vect3.x = rb.velocity.x;
        vect3.z = rb.velocity.z;

        vect3.y = spd;
        rb.velocity = vect3;
    }
    protected void SetZVel(float spd)
    {
        vect3.x = rb.velocity.x;
        vect3.y = rb.velocity.y;

        vect3.z = spd;
        rb.velocity = vect3;
    }
    
    protected void SetFwdVel(float spd)
    {
        vect3.x = trfm.forward.x * spd;
        vect3.y = rb.velocity.y;
        vect3.z = trfm.forward.z * spd;

        rb.velocity = vect3;
    }

    protected void AddXVel(float amount, float max = float.PositiveInfinity)
    {
        SetXVel(rb.velocity.x + amount);
        if ((amount > 0 && rb.velocity.x > max) || amount < 0 && rb.velocity.x < max) { SetXVel(max); }
    }
    protected void AddYVel(float amount, float max = float.PositiveInfinity)
    {
        vect3.x = 0;
        vect3.y = amount;
        vect3.z = 0;
        rb.velocity += vect3;

        if ((amount > 0 && rb.velocity.y > max) || amount < 0 && rb.velocity.y < max) { SetYVel(max); }
    }
    protected void AddZVel(float amount, float max = float.PositiveInfinity)
    {
        SetZVel(rb.velocity.z + amount);
        if ((amount > 0 && rb.velocity.z > max) || amount < 0 && rb.velocity.z < max) { SetZVel(max); }
    }
    protected void AddFwdVel(float amount, float max = float.PositiveInfinity)
    {
        vect3.x = rb.velocity.x + trfm.forward.x * amount;
        vect3.y = rb.velocity.y;
        vect3.z = rb.velocity.z + trfm.forward.z * amount;

        if (vect3.x * vect3.x + vect3.z * vect3.z < amount * amount)
        {
            rb.velocity += vect3;
        }
        else
        {
            SetFwdVel(max);
        }
    }
}
