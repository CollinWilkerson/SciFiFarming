using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLibrary : MonoBehaviour
{
    public static WeaponData[] library;

    public void Awake()
    {
        //Debug.Log(gameObject.name + " awake");
        library = gameObject.GetComponents<WeaponData>();
    }
}
