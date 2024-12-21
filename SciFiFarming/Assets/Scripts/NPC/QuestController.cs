using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    public int kills = 0;
    public ItemType desiredType;
    public int desiredIndex;
    public InventorySlotController turnIn;
    public Button submitQuest;

    private void Start()
    {
        submitQuest.enabled = false;
    }

    public void OnTurnIn()
    {
        if(turnIn.isFilled && turnIn.type == desiredType && turnIn.GetLibraryIndex() == desiredIndex)
        {
            submitQuest.enabled = true;
        }
        else
        {
            submitQuest.enabled = false;
        }
    }

    public void SubmitQuest()
    {

    }

    
}
