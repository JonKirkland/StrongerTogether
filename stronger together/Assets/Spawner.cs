using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    GameObject[] enemies;
    public GameObject marcusPrefab;
    private int whichSpawnPoint;
    private bool currentlySpawning; //so coroutine doesnt spam run

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentlySpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        whichSpawnPoint = Random.Range(0, 5);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //print(enemies.Length);
        if(enemies.Length < 5 && currentlySpawning == false)
        {
            print("ITS TRUE");
            StartCoroutine(SpawnEnemy());
        }
        //instantiate prefab at spawnPoint[Random.Range(0,5]
    }
    private IEnumerator SpawnEnemy()
    {
        currentlySpawning = true;
        yield return new WaitForSeconds(Random.Range(0,2));
        Instantiate(marcusPrefab, spawnPoints[Random.Range(0,4)].transform.position, Quaternion.identity);
        print("marcus Spawn");
        currentlySpawning = false;
    }
}
