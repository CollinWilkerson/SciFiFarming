using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellScreenController : MonoBehaviour
{
    private List<(string name, int quantity, int value)> sellItems;
    private int total;
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
        //the first time the screen is enabled the Inventories won't have anything yet
        try
        {
            Debug.Log(playerInventory.slots[0]);
            Debug.Log(sellInventory.slots[0]);
        }
        catch
        {
            return;
        }
        Debug.Log("OnEnable");
        //pull the players inventory
        foreach (InventorySlotController s in PlayerController.clientPlayer.inventory.slots)
        {
            playerInventory.slots[s.controllerIndex].SetInventorySlot(s);
            if (!s.isFilled)
            {
                playerInventory.slots[s.controllerIndex].EmptyInventorySlot();
            }
        }
        //put anything in the sell inventory on the display
        PullFromInventory();
    }

    private void OnDisable()
    {
        //push this inventory to the players invetory
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
                return;
            }
            else if(s.type == ItemType.plant)
            {
                PlantData tempPlant = PlantLibrary.library[s.GetLibraryIndex()];
                AddItem((tempPlant.type, s.GetQuantity(), tempPlant.value));
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
        GameManager.instance.gold += total;
        total = 0;
        sellItems.Clear();
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
