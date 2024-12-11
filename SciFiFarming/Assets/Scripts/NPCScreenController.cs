using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class NPCScreenController : MonoBehaviourPun
{
    [SerializeField] private GameObject interactionScreen;
    private GameObject currentScreen;
    private float affinity; //from 0 - 100 how much the NPC likes the player
    private RackController[] racks;

    private void Awake()
    {
        currentScreen = interactionScreen;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.toolbar.gameObject.SetActive(false);

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
        currentScreen.SetActive(false);
        screen.SetActive(true);
        currentScreen = screen;
    }
    //TALK SCREEN CODE
    [Header("Talk Screen")]
    [SerializeField] private TextMeshProUGUI talkDialog;
    [HideInInspector] public bool talked = false;

    public void Chat()
    {
        if (!talked)
        {
            talkDialog.text = "Captain: Less talking, more working";
            affinity = Mathf.Clamp(affinity + 0.001f, affinity, 50f);
            talked = true; //talked should be set false by the cycle advance code
        }
    }

    public void Gift()
    {

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
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempPlant.type +
                        "\nGrowth Stages: " + (tempPlant.harvestStage + 1) +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.seed:
                    PlantData tempSeed = PlantLibrary.library[InventoryController.hand.GetLibraryIndex()];
                    cost = tempSeed.value/10 * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempSeed.type +
                        "\nGrowth Stages: " + (tempSeed.harvestStage + 1) +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.weapon:
                    WeaponData tempWeapon = WeaponLibrary.library[InventoryController.hand.GetLibraryIndex()];
                    cost = tempWeapon.value * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempWeapon.type +
                        "\nType: " + (tempWeapon.weaponType) +
                        "\nDamage: " + tempWeapon.damage +
                        "\nRate of Fire: " + tempWeapon.rateOfFire + "/s" +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.helmet:
                    HelmetData tempHelmet = EquipmentLibrary.helmetLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempHelmet.value * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempHelmet.type +
                        "\nDefence: " + tempHelmet.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.chestArmr:
                    ChestArmorData tempChest = EquipmentLibrary.chestArmorLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempChest.value * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempChest.type +
                        "\nDefence: " + tempChest.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.belt:
                    BeltData tempBelt = EquipmentLibrary.beltLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempBelt.value  * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempBelt.type +
                        "\nDefence: " + tempBelt.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.LegArmr:
                    LegArmorData tempLeg = EquipmentLibrary.legArmorLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempLeg.value * 2;
                    cost -= (int)(cost * affinity / 100);
                    itemDataText.text = "Name: " + tempLeg.type +
                        "\nDefence: " + tempLeg.defence +
                        "\n<b>Price: " + cost + "</b>";
                    break;
                case ItemType.boots:
                    BootsData tempBoots = EquipmentLibrary.bootsLibrary[InventoryController.hand.GetLibraryIndex()];
                    cost = tempBoots.value * 2;
                    cost -= (int)(cost * affinity / 100);
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
        if(InventoryController.hand != null && PersistentData.money >= cost)
        {
            //only adds 1 for now
            PlayerController.clientPlayer.inventory.AddItem(InventoryController.hand.type,
                InventoryController.hand.GetLibraryIndex(), 1);
            BuyDialog.text = "Captain: Thanks, now go be useful.";
            PersistentData.money -= cost;
        }
    }

    //SHIP UPGRADES
    [Header("Ship Upgrades")]
    [SerializeField] private TextMeshProUGUI bedButtonText;
    [SerializeField] private Button rackButton;
    [SerializeField] private TextMeshProUGUI rackButtonText;
    private int activeRacks = 1;
    private bool upgradeRack = false;
    private bool upgradeBed = false;

    public void SetRacks(RackController[] setRacks)
    {
        racks = new RackController[setRacks.Length];
        foreach (RackController rack in setRacks)
        {
            //Debug.Log("Rack Hidden");
            racks[rack.rackID] = rack;
            rack.gameObject.SetActive(false);
        }
        racks[0].gameObject.SetActive(true);
    }

    //attached to the buy button on the ship upgrade screen
    public void BuyShipUpgrade()
    {
        if (upgradeRack && PersistentData.money >= Mathf.Pow(2, activeRacks) * 2500)
        {
            PersistentData.money -= (int)(Mathf.Pow(2, activeRacks) * 2500);
            //BuyRack();
            photonView.RPC("BuyRack", RpcTarget.All);
        }
        else if (upgradeBed)
        {
            BedUpgrade();
        }
    }

    //sets a new rack active and increases the price of a new rack
    [PunRPC]
    private void BuyRack()
    {
        racks[activeRacks].gameObject.SetActive(true);
        activeRacks++;
        rackButtonText.text = "New\nHydroponics\n" + (int)(Mathf.Pow(2, activeRacks) * 2500) + "D";
        upgradeRack = false;
        if(activeRacks > racks.Length - 1)
        {
            rackButton.interactable = false;
        }
    }

    [PunRPC]
    private void BedUpgrade()
    {
        Debug.LogError("NOT IMPLEMENTED");
    }

    public void BedButton()
    {
        upgradeBed = true;
        upgradeRack = false;
    }

    public void UpgradeRackButton()
    {
        upgradeRack = true;
        upgradeBed = false;
    }

    //RACK UPGRADES
    [Header("Rack Upgrades")]
    [SerializeField] private TextMeshProUGUI retentionUpgradeText;
    [SerializeField] private Button[] rackButtons;
    private int selectedRack = -1;

    public void RackUpgradesScreenActive()
    {
        for(int i = 0; i < rackButtons.Length; i++)
        {
            if(i < activeRacks)
            {
                rackButtons[i].interactable = true;
            }
            else
            {
                rackButtons[i].interactable = false;
            }
        }
    }

    public void SelectRack(int rackNum)
    {
        selectedRack = rackNum;
        retentionUpgradeText.text = "Retention - " + 500 * racks[selectedRack].retentionTier;
    }

    public void UpgradeRetention()
    {
        if(selectedRack > -1 && selectedRack < activeRacks && PersistentData.money >= 500 * racks[selectedRack].retentionTier)
        {
            PersistentData.money -= 500 * racks[selectedRack].retentionTier;
            //NetworkUpgradeRetention()
            photonView.RPC("NetworkUpgradeRetention", RpcTarget.All);
        }
    }

    [PunRPC]
    private void NetworkUpgradeRetention()
    {
        racks[selectedRack].retention += 0.2f;
        racks[selectedRack].retentionTier++;
        selectedRack = -1;
    }
}
