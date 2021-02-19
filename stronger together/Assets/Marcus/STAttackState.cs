using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STAttackState : State
{
    //states to return
    public STPursueTargetState pursueTargetState;
    public GameObject theEnemy;
    public STDeadState deadState;
    STEnemyController enemyController;
    public float viewDistance;
    private float viewAngle = 100;
    public LayerMask viewMask;
    private void Start()
    {

        enemyController = theEnemy.GetComponent<STEnemyController>();


    }
    public override State RunCurrentState()
    {
        enemyController.anim.SetBool("isShooting", true);

        if (enemyController.isDead == true)
        {
            return deadState;
        }
        FaceTarget();
        if (Vector3.Distance(transform.position, enemyController.target.position) > viewDistance)//if distance between player and guard MORE than view distance
        {
            endAttackState();
            return pursueTargetState;
        }

        Vector3 dirToPlayer = (enemyController.target.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer); // returns smallest angle between them
        
        if (angleBetweenGuardAndPlayer > viewAngle / 2f) //if out viewing angle
        {
            endAttackState();
            return pursueTargetState;
        }
        
        if (Physics.Linecast(transform.position + transform.up * 2, enemyController.target.position, viewMask)) //if we  hit anything between these points (like and obstacle)
        {

            endAttackState();

            return pursueTargetState;
        }
        return this;
    }
    
    void FaceTarget()
    {
        Vector3 direction = (enemyController.target.position - enemyController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
    private void endAttackState()
    {
        enemyController.anim.SetBool("isShooting", false);

    }


    
}
