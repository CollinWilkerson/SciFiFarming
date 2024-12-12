using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickupController : MonoBehaviourPun
{
    [SerializeField] private ItemType type = ItemType.plant;
    [SerializeField] private int libraryIndex = 0;
    [SerializeField] private int quantity = 1;

    private void Update()
    {
        /*
        Debug.Log(Input.GetKeyDown(KeyCode.E));
        if (PlayerController.clientPlayer != null)
        {
            Debug.Log("Current Interactable: " + PlayerController.clientPlayer.currentInteractable);
            Debug.Log("Player is gameobject: " + (PlayerController.clientPlayer.currentInteractable == gameObject));
        }
        */
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        PlayerController.clientPlayer.inventory.AddItem(type, libraryIndex, quantity);
        photonView.RPC("MasterDestroy", RpcTarget.All);
    }



    [PunRPC]
    private void MasterDestroy()
    {
        gameObject.SetActive(false);
    }
}
