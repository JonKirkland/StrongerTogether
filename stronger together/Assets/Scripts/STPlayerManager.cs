using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPlayerManager : MonoBehaviour
{
    #region Singleton
    public static STPlayerManager instance;
    void Awake()
    {
        instance = this;
    }

    #endregion
    public GameObject player;
}
