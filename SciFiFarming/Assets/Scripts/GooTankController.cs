using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GooTankController : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private int gooPerBlob = 100;

    void Update()
    {
        //if the player is looking at goo and presses E they get goo
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;


            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, GameManager.interactables))
            {
                if (hit.collider.gameObject.CompareTag("Goo"))
                {
                    PersistentData.goo += gooPerBlob;
                    PhotonNetwork.Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
