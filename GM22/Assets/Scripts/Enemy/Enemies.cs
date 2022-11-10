using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    public NavMeshAgent agent; //reference to agent

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    
    //patrolling 
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    
    //attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        //search for player
        player = GameObject.Find("characterDONE4").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        //figure out what enemy should do depending on where player is
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }
    private void Patrolling()
    {
        if (!walkPointSet)  //find new walkpoint
            SearchWalkPoint();
        else
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) //walkpoint reached
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            Debug.Log("setting walkpt");
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        transform.LookAt(player);
        agent.SetDestination((player.position));
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //attack code//
            Debug.Log("attacking player");
           //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); 
            //invoke is used to implement the delay between attacks
        }
            
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    */
    
}