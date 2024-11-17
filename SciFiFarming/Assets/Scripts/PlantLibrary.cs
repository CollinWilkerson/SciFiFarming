using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour
{
    public static PlantData[] library;

    private void Awake()
    {
        library = gameObject.GetComponents<PlantData>();
    }
}
