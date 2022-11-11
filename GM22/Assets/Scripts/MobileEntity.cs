using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobileEntity : HPEntity
{
    [SerializeField] Transform trfm;
    [SerializeField] Rigidbody rb;
    Vector3 vect3;
    protected new void _Start()
    {
        base._Start();
    }

    public float lookRadius = 3f;

    private Transform target; //reference from enemy to player

    private NavMeshAgent agent; // reference to agent to move enemy
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        //convert game object to type transform
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("target " + target + target.position);
        Debug.Log("tranform" + transform.position);
        float distance = Vector3.Distance(target.position, transform.position);
        Debug.Log("distance " + distance);
        if (distance <= lookRadius)
        {
            //chase player
            agent.SetDestination(target.position);
            Debug.Log(agent.destination);
            Debug.Log(target.position);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
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
    

    protected void AddXVel(float amount, float max = float.PositiveInfinity)
    {
        SetXVel(rb.velocity.x + amount);
        if ((amount > 0 && rb.velocity.x > max) || amount < 0 && rb.velocity.x < max) { SetXVel(max); }
    }
    protected void AddYVel(float amount, float max = float.PositiveInfinity)
    {
        SetYVel(rb.velocity.y + amount);
        if ((amount > 0 && rb.velocity.y > max) || amount < 0 && rb.velocity.y < max) { SetYVel(max); }
    }
    protected void AddZVel(float amount, float max = float.PositiveInfinity)
    {
        SetZVel(rb.velocity.z + amount);
        if ((amount > 0 && rb.velocity.z > max) || amount < 0 && rb.velocity.z < max) { SetZVel(max); }
    }
}
