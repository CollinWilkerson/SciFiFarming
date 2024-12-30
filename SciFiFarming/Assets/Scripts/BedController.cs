using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BedController : MonoBehaviourPun
{
    private static int playersInBed = 0;
    private TextMeshProUGUI sleepText;
    private GameObject[] goos;
    private GameObject[] pickups;
    private GameObject[] spiders;

    private void Start()
    {
        GameManager.instance.SleepScreen.SetActive(false);
        goos = GameObject.FindGameObjectsWithTag("Goo");
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        spiders = GameObject.FindGameObjectsWithTag("Enemy");
        sleepText = GameManager.instance.SleepScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
{
        //Debug.Log(GameManager.instance.SleepScreen.activeSelf);
        //this did not work, check return values
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable != null && 
            PlayerController.clientPlayer.currentInteractable.CompareTag("Bed") && 
            !PlayerController.clientPlayer.rb.isKinematic)
        {
            //Debug.Log("bed");
            PlayerController.clientPlayer.rb.isKinematic = true;
            GameManager.instance.SleepScreen.SetActive(true);
            //Debug.Log("Set Screen: " + GameManager.instance.SleepScreen.activeSelf);
            photonView.RPC("GetInBed", RpcTarget.All);
        }
        else if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.rb.isKinematic)
        {
            //Debug.Log("unbed");
            PlayerController.clientPlayer.rb.isKinematic = false;
            photonView.RPC("GetOutBed", RpcTarget.All);
            GameManager.instance.SleepScreen.SetActive(false);
            //Debug.Log("Set Screen: " + GameManager.instance.SleepScreen.activeSelf);
        }
    }

    [PunRPC]
    private void GetInBed()
    {
        //Debug.Log("eepy: " + playersInBed);
        playersInBed ++;
        sleepText.text = playersInBed + "/4 Players Sleeping";
        if(playersInBed == PhotonNetwork.PlayerList.Length)
        {
            CycleAdvance();
        }
    }

    [PunRPC]
    private void GetOutBed()
    {
        playersInBed--;
        sleepText.text = playersInBed + "/4 Players Sleeping";
    }

    private void CycleAdvance()
    {
        //Debug.Log("Cycle Advance");
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
            //Debug.Log("Spider");
            g.SetActive(true);
            g.GetComponent<BugEnemy>().Revive();
        }

        PlayerController.clientPlayer.photonView.RPC("Heal", RpcTarget.All, 10000f);

        //reset bed
        //playersInBed = 0;
        //PlayerController.clientPlayer.rb.isKinematic = false;
        sleepText.text = "You got a good night of rest.\nPress E to wake up";
        //Debug.Log("Set Screen: " + GameManager.instance.SleepScreen.activeSelf);
    }
}
