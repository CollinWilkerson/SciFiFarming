using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingMachine : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int reward;
    [SerializeField] private int chance;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.clientPlayer.currentInteractable == gameObject)
        {
            if(PersistentData.money >= price)
            {
                PersistentData.money -= price;

                if(Random.Range(0, chance) == 0)
                {
                    PersistentData.money += reward;
                }

                GameManager.moneyText.text = PersistentData.money + "D";
            }
        }
    }
}
