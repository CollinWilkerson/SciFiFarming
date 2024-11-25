using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour
{
    public static PlantData[] library;
    public static PlantLibrary instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log(gameObject.name + " awake");
        library = gameObject.GetComponents<PlantData>();
    }
}
