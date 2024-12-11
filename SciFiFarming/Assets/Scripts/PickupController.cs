using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickupController : MonoBehaviour
{
    [SerializeField] private ItemType type = ItemType.plant;
    [SerializeField] private int libraryIndex = 0;
    [SerializeField] private int quantity = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        PlayerController.clientPlayer.inventory.AddItem(type, libraryIndex, quantity);
        PhotonNetwork.Destroy(gameObject);
    }
}
