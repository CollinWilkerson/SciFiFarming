using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour
{
    public static PlantData[] library;

    public void Awake()
    {
        //DaDebug.Log(gameObject.name + " awake");
        library = gameObject.GetComponents<PlantData>();
    }
}
