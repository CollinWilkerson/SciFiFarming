using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform teleportLocation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteractable();
        }
    }

    void CheckInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f, LayerMask.GetMask("Interactable")))
        {
            if (hit.collider != null && hit.collider.CompareTag("Teleport"))
            {
                TeleportPlayer();
            }
        }
    }

    void TeleportPlayer()
    {
        if (teleportLocation != null)
        {
            Debug.Log($"Teleporting to: {teleportLocation.name} at position {teleportLocation.position}");
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = teleportLocation.position;
                player.transform.rotation = teleportLocation.rotation;
            }
        }
        else
        {
            Debug.LogError("Teleport location is null!");
        }
    }
}