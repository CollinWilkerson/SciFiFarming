using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestArmorData : MonoBehaviour
{
    [Header("General")]
    public int equipmentIndex = 0;
    public int value = 1000;
    public string type = "Rust Bucket";
    public string model;
    public Sprite inventroySprite;

    [Header("Equipment Stats")]
    public float defence = 0;
}
