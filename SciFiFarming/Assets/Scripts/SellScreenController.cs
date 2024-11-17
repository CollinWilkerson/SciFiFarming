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

    private void Start()
    {
        sellItems = new List<(string name, int quantity, int value)>();
        totalText.text = "Total 0 0D";
        partsText.text = "";
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
