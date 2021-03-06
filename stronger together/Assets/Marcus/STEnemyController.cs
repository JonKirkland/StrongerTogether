﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class STEnemyController : MonoBehaviour
{
    public Animator anim;
    public float lookRadius = 10f;
    public float foundRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public float eSpeed;
    private Vector3 lastPosition;
    public bool foundPlayer;
    public bool staggered;
    private bool marcusIsKilled = false;
    public GameObject hand;
    public Transform bulletSpawnPoint;
    public Transform bullet;

    public float waitTime = 1f;
    public float turnSpeed = 90;
    public float viewDistance;
    private float viewAngle = 100;
    public float enemyBulletForce = 200f;
    public LayerMask viewMask;

    public AudioSource enemyShoot;
    public bool isDead = false; //used for stoppping coroutine
    

    
    public bool inPatrolState = true;
    void Start()
    {
        print(inPatrolState);
        //isDead = false;
        marcusIsKilled = false;
        
        staggered = false;
        anim = GetComponent<Animator>();
        
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player").transform;
            

        }
        

        //target = STPlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        setRigidbodyState(true);
        setColliderState(false);
        
        
       
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        eSpeed = Mathf.Lerp(eSpeed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.1f);
        lastPosition = transform.position;
    }
    void Update()
    {
        
        float distance = Vector3.Distance(target.position, transform.position);

        //anim.SetBool("isMoving", true);


        



        
        if (GameObject.FindWithTag("Player") != null)
        {
            if (target != GameObject.FindWithTag("Player").transform)
            {
                target = GameObject.FindWithTag("Player").transform;
            }
            if (target != null)
            {
                target = GameObject.FindWithTag("Player").transform;
            }
            


        }
        if (isDead == true && marcusIsKilled == false)
        {
            Die();
        }
    }
    public void PlayShootSound()
    {
        enemyShoot.Play();
    }
   
    public void Die()
    {
        anim.enabled = false;
        agent.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        marcusIsKilled = true;
        isDead = true;
        transform.gameObject.tag = "Dead";
        StartCoroutine(ImmaHeadOut());
    }
    private IEnumerator ImmaHeadOut()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        var shootBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        shootBullet.GetComponent<Rigidbody>().velocity = (target.position - bulletSpawnPoint.transform.position).normalized * enemyBulletForce;
        
        
    }
    
    public void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state; //set rigidbodies to the state we pass it

        }
        GetComponent<Rigidbody>().isKinematic = state;
        
    }
    
    public void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state; 

        }

        GetComponent<Collider>().enabled = !state;
    }
    
    
    
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    
    
    public IEnumerator FollowPath(Vector3[] waypoints)
    {
        
        transform.position = waypoints[0];
        
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);
        while (isDead == false && inPatrolState == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, 3 * Time.deltaTime);
            if(transform.position == targetWaypoint)
            {   
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                print("starting to wait");
                yield return new WaitForSeconds(0);
                print("im done waiting");
                yield return StartCoroutine(TurnToFace(targetWaypoint));

            }
            yield return null;
        }
    }

    public IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f && inPatrolState == true)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
            
    }
    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.up * 2, transform.forward * viewDistance);
    }
    */
}
