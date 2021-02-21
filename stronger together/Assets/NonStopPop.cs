using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStopPop : MonoBehaviour
{


    // stop music from restarting on new scene
    void Awake()
    {
        //array is for music doesnt go over itself when returning to menu
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);

        }

        DontDestroyOnLoad(this.gameObject);


    }



}