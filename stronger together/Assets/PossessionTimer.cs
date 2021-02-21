using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionTimer : MonoBehaviour
{
    // Start is called before the first frame update
    //public float possessionTimer = 15f;
    public GameObject playerPrefab;
    public GameObject deadMarcus;
    STPlayerInfo playerHealth;
    void Start()
    {
        //StartCoroutine(Timer());
        playerHealth = gameObject.GetComponent<STPlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.health < 9)
        {
            StartCoroutine(Timer());
        }
    }
    public IEnumerator Timer()
    {
        //yield return new WaitForSeconds(possessionTimer);
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        Instantiate(deadMarcus, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}
