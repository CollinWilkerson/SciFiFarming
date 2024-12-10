using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCScreenController : MonoBehaviour
{
    [SerializeField] private GameObject interactionScreen;
    private GameObject currentScreen;
    private InventorySlotController itemToBuy;
    private RackController[] racks;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.toolbar.gameObject.SetActive(false);

        currentScreen = interactionScreen;
        InventoryController.hand = null;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.toolbar.gameObject.SetActive(true);

        InventoryController.hand = null;
    }
    public void ScreenChange(GameObject screen)
    {
        Debug.Log("Clicked");
        screen.SetActive(true);
        currentScreen.SetActive(false);
        currentScreen = screen;
    }

    //BUY SCREEN CODE
    [Header("Buy Screen")]
    [SerializeField] private TextMeshProUGUI itemDataText;
    [SerializeField] private TextMeshProUGUI BuyDialog;

    private int cost = 0;
    public void SelectItem()
    {
        if (InventoryController.hand != null)
        {
            switch (InventoryController.hand.type)
            {
                case ItemType.plant:
                    PlantData tempPlant = PlantLibrary.library[InventoryController.hand.GetLibraryIndex()];
                    cost = tempPlant.value * 2;
                    itemDataText.text = "Name: " + tempPlant.type +
                        "\nGrowth Stages: " + (tempPlant.harvestStage + 1) +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.seed:
                    PlantData tempSeed = PlantLibrary.library[InventoryController.hand.GetLibraryIndex()];
                    cost = tempSeed.value/10 * 2;
                    itemDataText.text = "Name: " + tempSeed.type +
                        "\nGrowth Stages: " + (tempSeed.harvestStage + 1) +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.weapon:
                    WeaponData tempWeapon = WeaponLibrary.library[InventoryController.hand.GetLibraryIndex()];
                    cost = tempWeapon.value * 2;
                    itemDataText.text = "Name: " + tempWeapon.type +
                        "\nType: " + (tempWeapon.weaponType) +
                        "\nDamage: " + tempWeapon.damage +
                        "\nRate of Fire: " + tempWeapon.rateOfFire + "/s" +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.helmet:
                    HelmetData tempHelmet = EquipmentLibrary.helmetLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempHelmet.value * 2;
                    itemDataText.text = "Name: " + tempHelmet.type +
                        "\nDefence: " + tempHelmet.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.chestArmr:
                    ChestArmorData tempChest = EquipmentLibrary.chestArmorLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempChest.value * 2;
                    itemDataText.text = "Name: " + tempChest.type +
                        "\nDefence: " + tempChest.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.belt:
                    BeltData tempBelt = EquipmentLibrary.beltLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempBelt.value  * 2;
                    itemDataText.text = "Name: " + tempBelt.type +
                        "\nDefence: " + tempBelt.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.LegArmr:
                    LegArmorData tempLeg = EquipmentLibrary.legArmorLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempLeg.value * 2;
                    itemDataText.text = "Name: " + tempLeg.type +
                        "\nDefence: " + tempLeg.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.boots:
                    BootsData tempBoots = EquipmentLibrary.bootsLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempBoots.value * 2;
                    itemDataText.text = "Name: " + tempBoots.type +
                        "\nDefence: " + tempBoots.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
            }
        }
    }

    /// <summary>
    /// triggers on buy button to buy item
    /// </summary>
    public void BuyItem()
    {
        if(InventoryController.hand != null && PersistentData.money > cost)
        {
            //only adds 1 for now
            PlayerController.clientPlayer.inventory.AddItem(InventoryController.hand.type,
                InventoryController.hand.GetLibraryIndex(), 1);
            BuyDialog.text = "Captain: Thanks, now go be useful.";
            PersistentData.money -= cost;
        }
    }

    //SHIP UPGRADES
    
    public void SetRacks(RackController[] setRacks)
    {
        racks = setRacks; 
        foreach (RackController rack in racks)
        {
            Debug.Log("Rack Hidden");
            rack.gameObject.SetActive(false);
        }
        racks[0].gameObject.SetActive(true);
    }
}
