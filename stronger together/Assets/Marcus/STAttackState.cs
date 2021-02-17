using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STAttackState : State
{
    //states to return

    private void Start()
    {
        
        
     
        
    }
    public override State RunCurrentState()
    {

        return this;
    }
    /*
    void FaceTarget()
    {
        Vector3 direction = (enemyController.target.position - enemyController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void StandStill()
    {
        //a function to stop enemy sliding too close
        if (randomNumber >= 4 && randomNumber <= 6 && closeGap == false)
        {
            enemyController.agent.SetDestination(transform.position);
        }
    }
    */
}
