using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLibrary : MonoBehaviour
{
    public static HelmetData[] helmetLibrary;

    public void Awake()
    {
        //Debug.Log(gameObject.name + " awake");
        helmetLibrary = gameObject.GetComponents<HelmetData>();
    }
}
