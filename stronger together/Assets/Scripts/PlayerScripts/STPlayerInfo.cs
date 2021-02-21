using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPlayerInfo : MonoBehaviour
{
    public int health;
    GameObject canvas;
    public int maxHealth;
    GameOver gameOver;
    void Start()
    {
        health = maxHealth;
        canvas = GameObject.FindWithTag("UI");
        gameOver = canvas.GetComponent<GameOver>();

    }
    void Update()
    {
        if(health < 4)
        {
            gameOver.playerIsDead = true;
        }
    }

    public void Hurt(int damage)
    {
        health -= damage;
        print("Health: " + health);
        
        
    }
}
