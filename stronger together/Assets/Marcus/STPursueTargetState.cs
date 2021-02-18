using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPursueTargetState : State
{
    public STAttackState attackState;
    STEnemyController enemyController;
    public GameObject theEnemy;
    public LayerMask viewMask;
    
    public float viewDistance;
    private float viewAngle = 100;

    private void Start()
    {
        enemyController = theEnemy.GetComponent<STEnemyController>();
        
    }
    public override State RunCurrentState()
    {
        //float distance = Vector3.Distance(enemyController.target.position, transform.position);
        //print(distance);

        enemyController.agent.SetDestination(enemyController.target.position);
        enemyController.anim.SetBool("isMoving", true);

        if (Vector3.Distance(transform.position, enemyController.target.position) < viewDistance)//if distance between player and guard less than view distance
        {
            Vector3 dirToPlayer = (enemyController.target.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer); // returns smallest angle between them
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) //if within viewing angle
            {
                if (!Physics.Linecast(transform.position + transform.up * 2, enemyController.target.position, viewMask)) //if we dont hit anything between these points (like and obstacle)
                {
                    
                    
                    //stop walking animation
                    enemyController.anim.SetBool("isMoving", false);
                    //start chase
                    enemyController.agent.SetDestination(transform.position);
                    return this;
                }
            }
        }

        
        return this;
    }
}
