using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCScreenController : MonoBehaviour
{
    [SerializeField] private GameObject currentScreen;
    private InventorySlotController itemToBuy;

    private void OnEnable()
    {
        InventoryController.hand = null;
    }
    public void ScreenChange(GameObject screen)
    {
        screen.SetActive(true);
        currentScreen.SetActive(false);
        currentScreen = screen;
    }

    //BUY SCREEN CODE
    [Header("Buy Screen")]
    [SerializeField] private TextMeshProUGUI itemDataText;

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
                    cost = tempWeapon.value / 10 * 2;
                    itemDataText.text = "Name: " + tempWeapon.type +
                        "\nType: " + (tempWeapon.weaponType) +
                        "\nDamage: " + tempWeapon.damage +
                        "\nRate of Fire: " + tempWeapon.rateOfFire + "/s" +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.helmet:
                    HelmetData tempHelmet = EquipmentLibrary.helmetLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempHelmet.value / 10 * 2;
                    itemDataText.text = "Name: " + tempHelmet.type +
                        "\nDefence: " + tempHelmet.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
            }
        }
    }

    public void BuyItem()
    {
        if(InventoryController.hand != null)
        {

        }
    }

}
