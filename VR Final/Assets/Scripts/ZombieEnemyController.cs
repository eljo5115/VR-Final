using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool hasAttacked;

    //Retreating
    private bool isRetreating;
    private Vector3 retreatPoint;
    private bool retreatPointSet;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, inLightRange;

    GameManager gameManager;

    void Awake()
    {
        player = GameObject.Find("VR Wheelchair/Camera Offset").transform;
        agent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(inLightRange) RunAway();
        if(!playerInAttackRange && !playerInSightRange) Patrolling();
        if(!playerInAttackRange && playerInSightRange) ChasePlayer();
        if(!playerInAttackRange && !playerInSightRange) AttackPlayer();
    }
    private void Patrolling()
    {
        agent.speed = 2;
        if(!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float RandomZ = Random.Range(-walkPointRange,walkPointRange);
        float RandomX = Random.Range(-walkPointRange,walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z+RandomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);

        if (!hasAttacked)
        {
            //attack

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = 5;
    }
    public void RunAway()
    {
        if(!retreatPointSet) SearchWalkPoint();
        if(retreatPointSet)
        {
            agent.SetDestination(retreatPoint);
        }
    }
    private void SearchRetreatPoint()
    {
        float RandomZ = Random.Range(-walkPointRange,walkPointRange);
        float RandomX = Random.Range(-walkPointRange,walkPointRange);

        retreatPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z+RandomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && Physics.CheckSphere(transform.position, sightRange, whatIsPlayer))
        {
            retreatPointSet = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,sightRange);

    }
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.gameObject.CompareTag("Player"))
        {
            gameManager.LoadMainMenu();
        }
    }
}
