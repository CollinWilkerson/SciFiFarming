using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private string sceneToLoad;

    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            SwitchScenes();
        }
    }

    private void SwitchScenes()
    {
        PersistentData.StoreInList(PlayerController.clientPlayer.inventory);
        PersistentData.health = PlayerController.clientPlayer.health;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
        }
    }
}
