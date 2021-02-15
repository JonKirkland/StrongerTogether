using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int health;
    
    public int maxHealth;
    void Start()
    {
        health = maxHealth;
        
    }

    public void Hurt(int damage)
    {
        health -= damage;
        print("Health: " + health);
        
    }
}
