using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellScreenController : MonoBehaviour
{
    private List<(string name, int quantity, int value)> sellItems;
    private int total;
    private bool firstActive = true;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI partsText;
    [SerializeField] private InventoryController playerInventory;
    [SerializeField] private InventoryController sellInventory;

    private void Start()
    {
        sellItems = new List<(string name, int quantity, int value)>();
        totalText.text = "Total 0 0D";
        partsText.text = "";
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (firstActive)
        {
            //Debug.Log("I Quit");
            return;
        }

        //Debug.Log("OnEnable");
        Cursor.lockState = CursorLockMode.None;
        GameManager.toolbar.gameObject.SetActive(false);

        //pull the players inventory
        foreach (InventorySlotController s in PlayerController.clientPlayer.inventory.slots)
        {
            //return when the inventory goes further than the player THIS MAY CAUSE BUGS
            if(s.controllerIndex > playerInventory.slots.Length - 1)
            {
                return;
            }
            if (!s.isFilled)
            {
                playerInventory.slots[s.controllerIndex].EmptyInventorySlot();
                continue;
            }
            playerInventory.slots[s.controllerIndex].SetInventorySlot(s);
        }
        //put anything in the sell inventory on the display
        //PullFromInventory();
    }

    private void OnDisable()
    {
        if (firstActive)
        {
            //Debug.Log("I Quit");
            firstActive = false;
            return;
        }
        //push this inventory to the players invetory
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.toolbar.gameObject.SetActive(true);

        foreach (InventorySlotController s in playerInventory.slots)
        {
            PlayerController.clientPlayer.inventory.slots[s.controllerIndex].SetInventorySlot(s);
            if (!s.isFilled)
            {
                PlayerController.clientPlayer.inventory.slots[s.controllerIndex].EmptyInventorySlot();
            }
        }
    }

    public void PullFromInventory()
    {
        //empty the total, then refill it THIS IS INEFFICIENT
        total = 0;
        sellItems.Clear();
        foreach (InventorySlotController s in sellInventory.slots)
        {
            if (!s.isFilled)
            {
                continue;
            }
            else if(s.type == ItemType.plant)
            {
                PlantData tempPlant = PlantLibrary.library[s.GetLibraryIndex()];
                AddItem((tempPlant.type, s.GetQuantity(), tempPlant.value));
            }
            else if(s.type == ItemType.seed)
            {
                PlantData tempPlant = PlantLibrary.library[s.GetLibraryIndex()];
                AddItem((tempPlant.type + " seed", s.GetQuantity(), tempPlant.value/10));
            }
            else if(s.type == ItemType.weapon)
            {
                WeaponData tempWeapon = WeaponLibrary.library[s.GetLibraryIndex()];
                AddItem((tempWeapon.type, s.GetQuantity(), tempWeapon.value));
            }
            else if(s.type == ItemType.helmet)
            {
                HelmetData tempHelmet = EquipmentLibrary.helmetLibrary[s.GetLibraryIndex()];
                AddItem((tempHelmet.type, s.GetQuantity(), tempHelmet.value));
            }
        }
    }

    public void AddItem((string name, int quantity, int value) item)
    {
        sellItems.Add(item);
        total += item.value * item.quantity;
        ScreenUpdate();
    }

    public void RemoveItem(int index)
    {
        (string name, int quantity, int value) item = sellItems[index];
        total -= item.value * item.quantity;
        sellItems.RemoveAt(index);
        ScreenUpdate();
    }

    public void Sell()
    {
        PersistentData.money += total;
        GameManager.moneyText.text = PersistentData.money + "D";
        total = 0;
        sellItems.Clear();
        if (Leaderboard.instance != null)
        {
            Leaderboard.instance.SetLeaderboardEntry(PersistentData.money);
        }
        ScreenUpdate();
        foreach(InventorySlotController s in sellInventory.slots)
        {
            s.EmptyInventorySlot();
        }
    }

    private void ScreenUpdate()
    {
        int totalQuantity = 0;
        partsText.text = "";
        foreach ((string name, int quantity, int value) i in sellItems)
        {
            partsText.text += "\n" + i.name + " " + i.quantity + " " + i.value + "D";
            totalQuantity += i.quantity;
        }
        totalText.text = "Total " + totalQuantity + " " + total + "D";
    }
}
