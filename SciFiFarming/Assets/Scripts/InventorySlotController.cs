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
    private ItemType type;
    private int libraryIndex;
    private Image itemImage;
    public bool DebugFill;

    private void Awake()
    {
        itemImage = gameObject.GetComponent<Image>();
    }
    private void Start()
    {
        if (DebugFill)
        {
            SetInventorySlot(ItemType.plant, 0);
        }
    }

    /// <summary>
    /// fill slot by value
    /// </summary>
    /// <param name="addType"></param>
    /// <param name="addIndex"></param>
    public void SetInventorySlot(ItemType addType, int addIndex)
    {
        //checks for invalid idex
        if(addIndex < 0)
        {
            return;
        }
        type = addType;
        libraryIndex = addIndex;
        isFilled = true;
        if(type == ItemType.plant)
        {
            Debug.Log("Onion");
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
        }
    }

    /// <summary>
    /// copy another inventory slot
    /// </summary>
    /// <param name="other"></param>
    public void SetInventorySlot(InventorySlotController other)
    {
        type = other.type;
        libraryIndex = other.libraryIndex;
        isFilled = true;
        if (type == ItemType.plant)
        {
            Debug.Log("Onion");
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
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
}
