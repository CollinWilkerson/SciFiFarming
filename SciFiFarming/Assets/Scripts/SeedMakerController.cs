using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMakerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            makeSeed();
        }
    }
    public void makeSeed()
    {
        if(TempInventory.inventory.Count == 0)
        {
            Debug.Log("Nothing");
            return;
        }
        for (int i = 1; i < TempInventory.inventory[0].quantity; i++)
        {
            Debug.Log("seed added");
            TempInventory.AddSeed(TempInventory.inventory[0].plantType);
        }

        TempInventory.inventory.RemoveAt(0);
    }
}
