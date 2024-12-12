using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellObjController : MonoBehaviour
{
    [SerializeField] private GameObject shippingScreen;
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.clientPlayer == null)
        {
            return;
        }

        if (PlayerController.clientPlayer.currentInteractable == gameObject && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(!shippingScreen.activeSelf);
            shippingScreen.SetActive(!shippingScreen.activeSelf);

            if (shippingScreen.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (PlayerController.clientPlayer.currentInteractable != gameObject && shippingScreen.activeSelf)
        {
            shippingScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }


    }    
}
