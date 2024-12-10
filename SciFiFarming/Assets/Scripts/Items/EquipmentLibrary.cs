using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLibrary : MonoBehaviour
{
    public static HelmetData[] helmetLibrary;
    public static ChestArmorData[] chestArmorLibrary;
    public static BeltData[] beltLibrary;
    public static LegArmorData[] legArmorLibrary;
    public static BootsData[] bootsLibrary;

    public void Awake()
    {
        //Debug.Log(gameObject.name + " awake");
        helmetLibrary = gameObject.GetComponents<HelmetData>();
        chestArmorLibrary = gameObject.GetComponents<ChestArmorData>();
        beltLibrary = gameObject.GetComponents<BeltData>();
        legArmorLibrary = gameObject.GetComponents<LegArmorData>();
        bootsLibrary = gameObject.GetComponents<BootsData>();
    }
}
