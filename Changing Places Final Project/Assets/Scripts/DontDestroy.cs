using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    public static DontDestroy GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {           // If the static var is null
            instance = this;            // assign the script to it
            DontDestroyOnLoad(gameObject); // Keep the script over scene loadings
        }
        else if (instance != this)
        {        // if the variable is not this script
            Destroy(gameObject);            // Destroy the object
        }
    }
}
