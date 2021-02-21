using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ImmaHeadOut());
    }

    private IEnumerator ImmaHeadOut()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }
}
