using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STAttackState : State
{
    //states to return
    public STPursueTargetState pursueTargetState;
    public GameObject theEnemy;
    STEnemyController enemyController;
    private void Start()
    {

        enemyController = theEnemy.GetComponent<STEnemyController>();


    }
    public override State RunCurrentState()
    {

        return this;
    }
    
    void FaceTarget()
    {
        Vector3 direction = (enemyController.target.position - enemyController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }


    
}
