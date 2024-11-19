using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    plant,
    seed
}

public class InventorySlotController : MonoBehaviour
{
    [HideInInspector] public bool isFilled = false;
    [HideInInspector] public int controllerIndex;
    public Button button;
    public ItemType type;
    private int libraryIndex = -1;
    private int quantity;
    private Image itemImage;
    public bool DebugFill;

    private void Awake()
    {
        itemImage = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
    }
    private void Start()
    {
        if (DebugFill)
        {
            SetInventorySlot(ItemType.plant, 0, 1);
        }
    }

    /// <summary>
    /// fill slot by value
    /// </summary>
    /// <param name="addType"></param>
    /// <param name="addIndex"></param>
    /// <param name="addQuantity"></param>
    public void SetInventorySlot(ItemType addType, int addIndex, int addQuantity)
    {
        //checks for invalid idex
        if(addIndex < 0)
        {
            return;
        }
        type = addType;
        libraryIndex = addIndex;
        quantity = addQuantity;
        isFilled = true;
        if(type == ItemType.plant)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
        }
        if (type == ItemType.seed)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].seedSprite;
        }
    }

    /// <summary>
    /// copy another inventory slot
    /// </summary>
    /// <param name="other"></param>
    public void SetInventorySlot(InventorySlotController other)
    {
        if(other.libraryIndex == -1)
        {
            return;
        }
        type = other.type;
        libraryIndex = other.libraryIndex;
        quantity = other.quantity;
        isFilled = true;
        if (type == ItemType.plant)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
        }
        if (type == ItemType.seed)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].seedSprite;
        }
    }

    public int Use(int amount = 1)
    {
        if (quantity - amount > 0)
        {
            quantity -= amount;
            return amount;
        }
        else
        {
            EmptyInventorySlot();
            return quantity;
        }
    }

    /// <summary>
    /// clear out a slot
    /// </summary>
    public void EmptyInventorySlot()
    {
        libraryIndex = -1;
        isFilled = false;
        itemImage.sprite = null; 
    }

    public void OnItemSelect()
    {
        //if a slot with a filled inventory slot is selected, place it into the players "hand"
        if (InventoryController.hand == null && isFilled)
        {
            InventoryController.hand = this;
        }
        else if(InventoryController.hand != null)
        {
            //if the player selects an empty slot, place the item there
            if (!isFilled)
            {
                SetInventorySlot(InventoryController.hand);
                InventoryController.hand.EmptyInventorySlot();
                InventoryController.hand = null;
            }
        }
    }

    public int GetLibraryIndex()
    {
        return libraryIndex;
    }

    public int GetQuantity()
    {
        return quantity;
    }
}
