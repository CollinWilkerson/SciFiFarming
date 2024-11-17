using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellObjController : MonoBehaviour
{
    bool inRange = true;
    [SerializeField] private GameObject shippingScreen;
    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.I))
        {
            shippingScreen.SetActive(!shippingScreen.activeSelf);
        }
        else if (!inRange)
        {
            shippingScreen.SetActive(false);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
    */
}
