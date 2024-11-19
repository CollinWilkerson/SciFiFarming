using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMakerController : MonoBehaviour
{
    private bool inRange;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            makeSeed();
        }
    }
    public void makeSeed()
    {
        //this goes through the players inventory to find plants for now, I'll work on a toolbar for the player soon
        foreach(InventorySlotController s in PlayerController.clientPlayer.inventory.slots)
        {
            if (!s.isFilled)
            {
                continue;
            }
            if(s.type == ItemType.plant)
            {
                PlayerController.clientPlayer.inventory.AddItem(ItemType.seed, s.GetLibraryIndex(), s.GetQuantity());
                s.EmptyInventorySlot();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
