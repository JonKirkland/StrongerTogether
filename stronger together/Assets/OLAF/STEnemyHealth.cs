using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STEnemyHealth : MonoBehaviour
{
    public float health = 15f;
    public STEnemyController enemy;
    public void Awake()
    {
        
    }
    public void TakeDamage(float amnt)
    {
        health -= amnt;
        if (health <= 0)
        {
            enemy.Die();
        }
        print("enemy took damage");
    }


}
