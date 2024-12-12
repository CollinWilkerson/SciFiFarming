using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform teleportLocation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
        {
            PlayerController.clientPlayer.transform.position = teleportLocation.position;
        }
    }

}