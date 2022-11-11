using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MobileEntity
{
    public float lookRadius = 3f;

    private Transform target; //reference from enemy to player

    private NavMeshAgent agent; // reference to agent to move enemy
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        target = GameManager.playerControllerScr.trfm;
        //convert game object to type transform
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(target.position, trfm.position);
        if (distance <= lookRadius)
        {
            //chase player
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance) //makes sure enemy stops to attack and faces playe
            {
                //attack target
                FaceTarget();
                
            }
           
        }

    }

    protected void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; //direction vector from enemy to player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0, direction.z)); //target angle
        //for smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
