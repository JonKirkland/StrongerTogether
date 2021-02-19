using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float possessionTimer = 15f;
    public GameObject playerPrefab;
    public GameObject deadMarcus;
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(possessionTimer);
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        Instantiate(deadMarcus, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}
