using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventorySlotController[] slots;
    public static InventorySlotController hand;

    private void Awake()
    {
        //Debug.Log(gameObject.name + " Initialized");
        slots = gameObject.GetComponentsInChildren<InventorySlotController>();
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].controllerIndex = i;
        }
    }

    public void AddItem(ItemType addType, int addIndex, int addQuantity)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isFilled)
            {
                slots[i].SetInventorySlot(addType, addIndex, addQuantity);
                return;
            }
            else if(slots[i].type == addType && slots[i].GetLibraryIndex() == addIndex)
            {
                slots[i].AddToSlot(addQuantity);
                return;
            }
        }
        Debug.Log("Inventory Full");
    }

    public void ClearInventory()
    {
        foreach (InventorySlotController slot in slots)
        {
            slot.EmptyInventorySlot();
        }
    }
}
