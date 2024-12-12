using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BedController : MonoBehaviourPun
{
    private static int playersInBed = 0;
    private GameObject[] goos;
    private GameObject[] pickups;
    private GameObject[] spiders;

    private void Start()
    {
        goos = GameObject.FindGameObjectsWithTag("Goo");
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        spiders = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject && !PlayerController.clientPlayer.rb.isKinematic)
        {
            PlayerController.clientPlayer.rb.isKinematic = true;
            photonView.RPC("GetInBed", RpcTarget.All);
        }
        else if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.rb.isKinematic)
        {
            PlayerController.clientPlayer.rb.isKinematic = false;
            photonView.RPC("GetOutBed", RpcTarget.All);
        }
    }

    [PunRPC]
    private void GetInBed()
    {
        Debug.Log("eepy: " + playersInBed);
        playersInBed ++;
        if(playersInBed == PhotonNetwork.PlayerList.Length)
        {
            CycleAdvance();
        }
    }

    [PunRPC]
    private void GetOutBed()
    {
        playersInBed--;
    }

    private void CycleAdvance()
    {
        Debug.Log("Cycle Advance");
        foreach(RackController r in FindObjectsByType<RackController>(FindObjectsSortMode.None))
        {
            r.CycleAdvance();
        }

        GameManager.instance.npcScreen.talked = false;

        //sets all goo active, could be used to spawn goo in new random locations
        foreach(GameObject g in goos)
        {
            g.SetActive(true);
        }

        //spawns all pickups, could be check a layer or bool or something
        foreach (GameObject g in pickups)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in spiders)
        {
            g.SetActive(true);
        }

        PlayerController.clientPlayer.photonView.RPC("Heal", RpcTarget.All, 10000f);

        //reset bed
        playersInBed = 0;
        PlayerController.clientPlayer.rb.isKinematic = false;
    }
}
