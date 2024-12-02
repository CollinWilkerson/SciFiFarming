using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    handgun,
    shotgun,
    rifle,
}

public class WeaponData : MonoBehaviour
{
    [Header("General")]
    public int weaponIndex = 0;
    public int value = 1000;
    public string type = "P-Shooter";
    public string model;
    public Sprite inventroySprite;

    [Header("Weapon Stats")]
    public float damage = 10;
    public float range = 2;
    public float rateOfFire = 1;
    public WeaponType weaponType = WeaponType.handgun;

}
