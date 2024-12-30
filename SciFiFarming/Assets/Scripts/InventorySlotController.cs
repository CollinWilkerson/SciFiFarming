using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private TextMeshProUGUI quantityText;

    private Image itemImage;

    [Header("For armor slots only")]
    [SerializeField] private bool isArmor;
    private static bool armorInHand;
    [SerializeField] private ItemType slotType;

    private void Awake()
    {
        //Debug.Log("Initialized");
        itemImage = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        quantityText.text = "";
        //theres a better way to do this i just dont have time
        if (gameObject.GetComponent<WeaponData>() != null)
        {
            SetInventorySlot(ItemType.weapon, gameObject.GetComponent<WeaponData>().weaponIndex, 1);
        }
        else if (gameObject.GetComponent<HelmetData>() != null)
        {
            SetInventorySlot(ItemType.helmet, gameObject.GetComponent<HelmetData>().equipmentIndex, 1);
        }
        else if (gameObject.GetComponent<ChestArmorData>() != null)
        {
            SetInventorySlot(ItemType.chestArmr, gameObject.GetComponent<ChestArmorData>().equipmentIndex, 1);
        }
        else if (gameObject.GetComponent<LegArmorData>() != null)
        {
            SetInventorySlot(ItemType.LegArmr, gameObject.GetComponent<LegArmorData>().equipmentIndex, 1);
        }
        else if (gameObject.GetComponent<BootsData>() != null)
        {
            SetInventorySlot(ItemType.boots, gameObject.GetComponent<BootsData>().equipmentIndex, 1);

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
        if (addIndex < 0)
        {
            return;
        }
        type = addType;
        libraryIndex = addIndex;
        quantity = addQuantity;
        quantityText.text = "" + quantity;
        isFilled = true;
        if (type == ItemType.plant)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].inventorySprite;
        }
        else if (type == ItemType.seed)
        {
            itemImage.sprite = PlantLibrary.library[libraryIndex].seedSprite;
        }
        else if (type == ItemType.weapon)
        {
            itemImage.sprite = WeaponLibrary.library[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.helmet)
        {
            itemImage.sprite = EquipmentLibrary.helmetLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.chestArmr)
        {
            itemImage.sprite = EquipmentLibrary.chestArmorLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.belt)
        {
            itemImage.sprite = EquipmentLibrary.beltLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.LegArmr)
        {
            itemImage.sprite = EquipmentLibrary.legArmorLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.boots)
        {
            itemImage.sprite = EquipmentLibrary.bootsLibrary[libraryIndex].inventroySprite;
        }
    }

    /// <summary>
    /// copy another inventory slot
    /// </summary>
    /// <param name="other"></param>
    public void SetInventorySlot(InventorySlotController other)
    {
        //Debug.Log(other.libraryIndex);
        if (other.libraryIndex == -1)
        {
            return;
        }
        type = other.type;
        libraryIndex = other.libraryIndex;
        quantity = other.quantity;
        quantityText.text = "" + quantity;
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
        else if (type == ItemType.weapon)
        {
            itemImage.sprite = WeaponLibrary.library[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.helmet)
        {
            itemImage.sprite = EquipmentLibrary.helmetLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.chestArmr)
        {
            itemImage.sprite = EquipmentLibrary.chestArmorLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.belt)
        {
            itemImage.sprite = EquipmentLibrary.beltLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.LegArmr)
        {
            itemImage.sprite = EquipmentLibrary.legArmorLibrary[libraryIndex].inventroySprite;
        }
        else if (type == ItemType.boots)
        {
            itemImage.sprite = EquipmentLibrary.bootsLibrary[libraryIndex].inventroySprite;
        }
    }

    public int Use(int amount = 1)
    {
        if (quantity - amount > 0)
        {
            quantity -= amount;
            quantityText.text = "" + quantity;
            return amount;
        }
        else
        {
            EmptyInventorySlot();
            return quantity;
        }
    }

    public void AddToSlot(int amount)
    {
        if (!isFilled)
        {
            return;
        }
        quantity += amount;
        quantityText.text = "" + quantity;
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
        quantityText.text = "";
    }

    public void OnItemSelect()
    {
        //if a slot with a filled inventory slot is selected, place it into the players "hand"
        if (InventoryController.hand == null && isFilled)
        {
            InventoryController.hand = this;
            if (isArmor)
            {
                //update defence when player moves item
                armorInHand = true;
            }
        }
        else if (InventoryController.hand != null)
        {
            //if the player selects an empty slot, place the item there
            if (!isFilled)
            {
                if (isArmor)
                {
                    if (slotType != InventoryController.hand.type)
                    {
                        return;
                    }
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    SetInventorySlot(InventoryController.hand);
                    InventoryController.hand.Use((int) Mathf.Ceil(quantity / (float) 2));
                    Use(quantity / 2);
                    InventoryController.hand = null;
                }
                else
                {
                    SetInventorySlot(InventoryController.hand);

                    InventoryController.hand.EmptyInventorySlot();
                    InventoryController.hand = null;
                }
                //armor Management
                if (isArmor)
                {
                    Debug.Log("Armor Gained");
                    //update player defence
                    if (type == ItemType.helmet)
                    {
                        PlayerController.clientPlayer.defence += EquipmentLibrary.helmetLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.chestArmr)
                    {
                        PlayerController.clientPlayer.defence += EquipmentLibrary.chestArmorLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.belt)
                    {
                        PlayerController.clientPlayer.defence += EquipmentLibrary.beltLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.LegArmr)
                    {
                        PlayerController.clientPlayer.defence += EquipmentLibrary.legArmorLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.boots)
                    {
                        PlayerController.clientPlayer.defence += EquipmentLibrary.bootsLibrary[libraryIndex].defence;
                    }
                }
                if (armorInHand)
                {
                    Debug.Log("Armor Reduced");
                    if (type == ItemType.helmet)
                    {
                        PlayerController.clientPlayer.defence -= EquipmentLibrary.helmetLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.chestArmr)
                    {
                        PlayerController.clientPlayer.defence -= EquipmentLibrary.chestArmorLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.belt)
                    {
                        PlayerController.clientPlayer.defence -= EquipmentLibrary.beltLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.LegArmr)
                    {
                        PlayerController.clientPlayer.defence -= EquipmentLibrary.legArmorLibrary[libraryIndex].defence;
                    }
                    else if (type == ItemType.boots)
                    {
                        PlayerController.clientPlayer.defence -= EquipmentLibrary.bootsLibrary[libraryIndex].defence;
                    }
                    armorInHand = false;
                }
            }
            else //hand is filled and slot is filled
            {
                if (InventoryController.hand != this && type == InventoryController.hand.type && libraryIndex == InventoryController.hand.libraryIndex)
                {
                    AddToSlot(InventoryController.hand.quantity);
                    InventoryController.hand.EmptyInventorySlot();
                    InventoryController.hand = null;
                }
                else
                {
                    InventoryController.hand = this;
                    if (isArmor)
                    {
                        //update defence when player moves item
                        armorInHand = true;
                    }
                    else
                    {
                        armorInHand = false;
                    }
                }
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
