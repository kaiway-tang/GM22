using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    public float lookRadius = 3f;

    protected Transform target; //reference from enemy to player

    private UnityEngine.AI.NavMeshAgent agent; // reference to agent to move enemy
    protected new void Start()
    {
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        //convert game object to type transform
    }

    // Update is called once per frame
    void Update()
    {

    }
}
