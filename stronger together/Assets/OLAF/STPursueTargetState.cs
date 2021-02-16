using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPursueTargetState : State
{
    public STAttackState attackState;
    STEnemyController enemyController;
    public GameObject theEnemy;
    
    private void Start()
    {
        enemyController = theEnemy.GetComponent<STEnemyController>();
    }
    public override State RunCurrentState()
    {
        float distance = Vector3.Distance(enemyController.target.position, transform.position);
        print(distance);
        //enemyController.agent.SetDestination(enemyController.target.position);
        //run at player position, (or sprint) with more speed
        //if remainingDistance is large then sprint, however remaining distance doesnt work if you have no target, will have to get distance to player
        if (distance <= 5 && enemyController.agent.enabled)
        {
            
            enemyController.anim.SetBool("isSprinting", false);
            print("I have arrived");
            enemyController.agent.speed = 5;
            enemyController.agent.SetDestination(transform.position);
            return attackState;
        }
        if(distance >= 5 && enemyController.agent.enabled)
        {
            enemyController.agent.SetDestination(enemyController.target.position);
            enemyController.anim.SetBool("isSprinting", true);
            enemyController.agent.speed = 10;
            return this;
        }
        
        //if lose player, go to last seen position

        return this;
    }
}
