using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    private void Update()
    {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, GameManager.interactables))
            {
                if (hit.collider.gameObject.CompareTag("Pickup"))
                {
                    Pickup();
                }
            }
        }
    }

    public void Pickup()
    {
        PersistentData.money += 1000;
        GameManager.moneyText.text = PersistentData.money + "D";
    }
}
