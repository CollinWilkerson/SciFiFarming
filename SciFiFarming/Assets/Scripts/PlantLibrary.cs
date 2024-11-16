using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour
{
    public static PlantBehavior[] library;

    private void Awake()
    {
        library = gameObject.GetComponents<PlantBehavior>();
    }
}
