using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS CLASS ONLY CONTAINS AN NPC's 3D GAME BEHAVIOR, INTERACTION SCRIPTS ARE ELSWHERE
public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject interactionScreen;
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.clientPlayer == null)
        {
            return;
        }
        if (PlayerController.clientPlayer.currentInteractable == gameObject && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log(!interactionScreen.activeSelf);
            interactionScreen.SetActive(!interactionScreen.activeSelf);
        }
        else if (PlayerController.clientPlayer.currentInteractable != gameObject && interactionScreen.activeSelf)
        {
            interactionScreen.SetActive(false);
        }
        //this should also contain nav mesh routes for NPC's
    }
}
