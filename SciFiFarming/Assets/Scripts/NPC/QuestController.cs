using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestController : MonoBehaviour
{
    private int kills = 0;
    private Quest[] quests;
    private int currentQuest = 0;
    public Button dutyButton;
    public TextMeshProUGUI dialogue;
    private int interactions = -1;

    private void Start()
    {
        dutyButton.enabled = true;
        quests = GetComponents<Quest>();
    }

    public void OnTurnIn()
    {
        //this should be attached to the duty button so thats the only thing the player interacts with

        if (interactions + 1 < quests[currentQuest].acceptText.Length)
        {
            interactions++;
            dialogue.text = "Capitan: " + quests[currentQuest].acceptText[interactions];
            if(interactions == quests[currentQuest].acceptText.Length - 1)
            {
                GameManager.instance.questShortText.text = "-" + quests[currentQuest].shortText;
            }
            return;
        }
        else
        {
            foreach (InventorySlotController s in PlayerController.clientPlayer.inventory.slots)
            {
                if (s.isFilled && s.type == quests[currentQuest].desiredType &&
                s.GetLibraryIndex() == quests[currentQuest].desiredIndex)
                {
                    s.Use(1);
                    SubmitQuest();
                    return;
                }
            }
            foreach (InventorySlotController s in ToolbarController.instance.toolbar)
            {
                if (s.isFilled && s.type == quests[currentQuest].desiredType &&
                s.GetLibraryIndex() == quests[currentQuest].desiredIndex)
                {
                    s.Use(1);
                    SubmitQuest();
                    return;
                }
            }
        }
        dialogue.text = quests[currentQuest].missText;
    }

    public void SubmitQuest()
    {
        if (quests[currentQuest].cashReward)
        {
            PersistentData.money += quests[currentQuest].reward;
            GameManager.moneyText.text = PersistentData.money + "D";
        }
        else
        {
            PlayerController.clientPlayer.inventory.AddItem(quests[currentQuest].rewardType, quests[currentQuest].rewardIndex, quests[currentQuest].rewardQuantity);
        }

        dialogue.text = quests[currentQuest].completedText;
        interactions = -1;
        currentQuest += 1;

        if (currentQuest + 1 > quests.Length)
        {
            dutyButton.enabled = false;
        }
    }

    
}
