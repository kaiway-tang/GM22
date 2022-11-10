using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
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
        Debug.Log("target "  + target +  target.position);
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
}
