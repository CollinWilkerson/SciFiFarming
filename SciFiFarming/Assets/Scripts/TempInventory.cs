using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInventory : MonoBehaviour
{
    public static List<(int plantType, int quantity, int value)> inventory;

    private void Awake()
    {
        inventory = new List<(int plantType, int quantity, int value)>();
    }

    public static void ListAdd((int plantType, int quantity, int value) item)
    {
        inventory.Add(item);
    }

    public void sellAdd(SellScreenController controller)
    {
        if(inventory.Count == 0)
        {
            Debug.Log("Nothing to sell!");
            return;
        }
        controller.AddItem((PlantLibrary.library[inventory[0].plantType].type, inventory[0].quantity, inventory[0].value));
        inventory.RemoveAt(0);
    }
}
