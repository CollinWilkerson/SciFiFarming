using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GooController : MonoBehaviourPun
{
    // Update is called once per frame
    [SerializeField] private int gooPerBlob = 100;

    void Update()
    {
        //if the player is looking at goo and presses E they get goo
        if (Input.GetKeyDown(KeyCode.E))
        {


            if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
            {
                PersistentData.goo += gooPerBlob;
                photonView.RPC("MasterDestroy", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    private void MasterDestroy()
    {
            PhotonNetwork.Destroy(gameObject);
    }
}
