using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMakerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject &&
            ToolbarController.instance.activeTool.type == ItemType.plant)
        {
            makeSeed();
        }
    }
    public void makeSeed()
    {
        InventorySlotController s = ToolbarController.instance.activeTool;
        //makes a seed from the plant the player was carrying and empties the toolbar slot
        PlayerController.clientPlayer.inventory.AddItem(ItemType.seed, s.GetLibraryIndex(), 1);
        s.Use(1);
        ToolbarController.instance.ClearHand();
    }
}
