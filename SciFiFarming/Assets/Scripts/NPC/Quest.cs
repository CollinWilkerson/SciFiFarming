using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    fetch,
    elimination
}

public class Quest : MonoBehaviour
{
    public QuestType type;
    public bool cashReward;
    public int reward;
    public ItemType rewardType;
    public int rewardIndex;
    public int rewardQuantity;
    public string[] acceptText;
    public string missText;
    public string completedText;
    public string shortText;

    [Header("Fetch")]
    public ItemType desiredType;
    public int desiredIndex;

    [Header("Elimination")]
    public string elimType; //does nothing for now used to destigush bosses
    public int killCount;
}
