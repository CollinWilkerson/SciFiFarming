using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script stores any data we need to load across scenes
public class PersistentData : MonoBehaviour
{
    public static List<(ItemType type, int index, int quantity)> slotStorage;
    public static float goo;
    public static int money;
    public static string lastScene;
    public static PersistentData instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        if (slotStorage == null)
        {
            slotStorage = new List<(ItemType type, int index, int quantity)>();
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// store all items from an inventory to be accessed later in a new scene
    /// </summary>
    /// <param name="inventoryController"></param>
    public static void StoreInList(InventoryController inventoryController)
    {
        foreach (InventorySlotController s in inventoryController.slots)
        {
            if (s.isFilled)
            {
                slotStorage.Add((s.type, s.GetLibraryIndex(), s.GetQuantity()));
            }
        }
    }

    /// <summary>
    /// store a single item to be accessed later in a new scene
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    /// <param name="quantity"></param>
    public static void StoreInList(ItemType type, int index, int quantity)
    {
        slotStorage.Add((type, index, quantity));
    }

    /// <summary>
    /// set an inventory to the stored items
    /// </summary>
    /// <param name="inventory"></param>
    public static void SetInventoryFromList(InventoryController inventory)
    {
        foreach ((ItemType type, int index, int quantity) s in slotStorage)
        {
            Debug.Log("Execute");
            inventory.AddItem(s.type, s.index, s.quantity);
        }
        slotStorage.Clear();
    }
}