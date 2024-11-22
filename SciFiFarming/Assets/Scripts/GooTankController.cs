using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooTankController : MonoBehaviour
{
    // Update is called once per frame
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;


            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, GameManager.interactables))
            {
                if (hit.collider.gameObject.CompareTag("Goo"))
                {

                }
            }
        }
    }
}
