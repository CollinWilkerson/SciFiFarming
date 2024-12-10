using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    plant,
    seed,
    weapon,
    helmet,
    chestArmr,
    belt,
    LegArmr,
    boots,
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

    [Header("For armor slots only")]
    [SerializeField] private bool isArmor;
    [SerializeField] private ItemType slotType;

    private void Awake()
    {
        //Debug.Log("Initialized");
        itemImage = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        if(gameObject.GetComponent<WeaponData>() != null)
        {
            SetInventorySlot(ItemType.weapon, gameObject.GetComponent<WeaponData>().weaponIndex, 1);
        }
        else if(gameObject.GetComponent<HelmetData>() != null)
        {
            SetInventorySlot(ItemType.helmet, gameObject.GetComponent<HelmetData>().equipmentIndex, 1);
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
        if (type == ItemType.weapon)
        {
            itemImage.sprite = WeaponLibrary.library[libraryIndex].inventroySprite;
        }
        if(type == ItemType.helmet)
        {
            itemImage.sprite = EquipmentLibrary.helmetLibrary[libraryIndex].inventroySprite;
        }
    }

    /// <summary>
    /// copy another inventory slot
    /// </summary>
    /// <param name="other"></param>
    public void SetInventorySlot(InventorySlotController other)
    {
        //Debug.Log(other.libraryIndex);
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
            //Debug.Log("Item Image: " + itemImage.sprite);
            //Debug.Log("Plant Library sprite: " + PlantLibrary.library[libraryIndex].inventorySprite);
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
        }
        if (type == ItemType.seed)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].seedSprite;
        }
        if (type == ItemType.weapon)
        {
            itemImage.sprite = WeaponLibrary.library[libraryIndex].inventroySprite;
        }
        if (type == ItemType.helmet)
        {
            itemImage.sprite = EquipmentLibrary.helmetLibrary[libraryIndex].inventroySprite;
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
        //Debug.Log(itemImage.sprite);
        itemImage.sprite = null; 
    }

    public void OnItemSelect()
    {
        //if a slot with a filled inventory slot is selected, place it into the players "hand"
        if (InventoryController.hand == null && isFilled)
        {
            InventoryController.hand = this;
            if (isArmor)
            {
                //update defence
            }
        }
        else if(InventoryController.hand != null)
        {
            //if the player selects an empty slot, place the item there
            if (!isFilled)
            {
                if (isArmor)
                {
                    if(slotType != InventoryController.hand.type)
                    {
                        return;
                    }
                    else
                    {
                        //update player defence
                    }
                }
                SetInventorySlot(InventoryController.hand);
                InventoryController.hand.EmptyInventorySlot();
                InventoryController.hand = null;
            }
            else
            {
                InventoryController.hand = this;
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

    public void SetColor(Color c)
    {
        itemImage.color = c;
    }
}
