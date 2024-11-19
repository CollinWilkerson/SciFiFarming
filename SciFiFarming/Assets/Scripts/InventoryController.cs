using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventorySlotController[] slots;
    public static InventorySlotController hand;

    private void Awake()
    {
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
        }
        Debug.Log("Inventory Full");
    }
}
