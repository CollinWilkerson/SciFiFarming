using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == PlayerController.clientPlayer.gameObject)
        {
            PlayerController.clientPlayer.TakeDamage(10000);
        }
    }
}
