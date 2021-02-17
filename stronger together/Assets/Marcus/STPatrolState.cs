using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class STPatrolState : State
{
    public STPursueTargetState pursueTargetState;
    public bool canSeeThePlayer;
    public LayerMask viewMask;
    public Transform pathHolder;
    STEnemyController enemyController;
    public float viewDistance;
    private float viewAngle = 100;
    public Transform player;
    public Transform[] points;
    private int destPoint = 0;
    public GameObject theEnemy;
    
    private void Awake()
    {
        enemyController = theEnemy.GetComponent<STEnemyController>();
    }
    private void Start()
    {

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }
        
        StartCoroutine(enemyController.FollowPath(waypoints));
        print("test");
        print(destPoint);
    }
    public override State RunCurrentState()
    {


        return this;

    }

}
