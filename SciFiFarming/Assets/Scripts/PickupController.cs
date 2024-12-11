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
        Debug.Log(Input.GetKeyDown(KeyCode.E));
        Debug.Log(PlayerController.clientPlayer.currentInteractable == gameObject);
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        PlayerController.clientPlayer.inventory.AddItem(type, libraryIndex, quantity);
        photonView.RPC("MasterDestroy", RpcTarget.MasterClient);
    }



    [PunRPC]
    private void MasterDestroy()
    {
            PhotonNetwork.Destroy(gameObject);
    }
}
