using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    public Transform orientation;
    public LayerMask Enemy;
    public GameObject gunController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Possess();
    }
    public void Possess()
    {
        if (Input.GetKey(KeyCode.E))
        {
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, orientation.forward, out hit, 1f, Enemy))
            {
                print("I hit it");
                Instantiate(gunController, transform.position, Quaternion.identity);
                Destroy(hit.transform.gameObject);
                gameObject.SetActive(false);
                //Destroy(gameObject);
                StartCoroutine(kys());

            }
        }
    }
    public IEnumerator kys()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
