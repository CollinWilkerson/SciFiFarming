using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private ItemType type = ItemType.plant;
    [SerializeField] private int libraryIndex = 0;
    [SerializeField] private int quantity = 1;

    private void Update()
    {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, GameManager.interactables))
            {
                if (hit.collider.gameObject.CompareTag("Pickup"))
                {
                    Pickup();
                }
            }
        }
    }

    public void Pickup()
    {
        PlayerController.clientPlayer.inventory.AddItem(type, libraryIndex, quantity);
        Destroy(gameObject);
    }
}
