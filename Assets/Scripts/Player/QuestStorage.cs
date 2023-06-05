using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    //Link to the players stats
    public StatsStorage stats;
    //This controls how many quests the player can hold 
    public int QuestCount;
    public bool QuestLimit;
    //The place the taken Quests are stored
    public QuestsList[] CurQuests = new QuestsList[3];
    //When the game launches we check and update the list
    void Start()
    {
        UpdateCurQuests();
    }
    //This runs through all the Quests to see if its empty(0) or not
    public void UpdateCurQuests()
    {
        QuestCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if (CurQuests[i].QuestID != 0)
            {
                QuestCount += 1;
            }
        }
        //If all are taken then deny any more quests the player tries to buy
        if (QuestCount == 3)
        {
            QuestLimit = true;
        }
        else
            QuestLimit = false;
    }
    //If the player can add a quest then this will run and find the next empty slot to place it
    public void AddQuest(QuestsList quest)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CurQuests[i].QuestID == 0)
            {
                CurQuests[i] = quest;
                UpdateCurQuests();
                break;
            }
        }
    }
    //This will find a Quest and then remove it
    public void RemoveQuest(int QuestID)
    {
        bool FoundQuest = false;
        for (int i = 0; i < 3; i++)
        {
            if (CurQuests[i].QuestID == QuestID)
            {
                CurQuests[i] = null;
                FoundQuest = true;
            }
            //This will then shuffle them along the order
            else if (FoundQuest == true)
            {
                CurQuests[i - 1] = CurQuests[i];
            }
        }
        UpdateCurQuests();
    }
    //This will then give the rewards and remove the quest if the player completes all the objectives of said quest
    public void FinishQuest(int QuestID)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CurQuests[i].QuestID == QuestID)
            {
                stats.Currency += CurQuests[i].Reward;
                stats.Reputation[CurQuests[i].Faction] += CurQuests[i].FactionReward;
                stats.UpdateMoney();
                stats.UpdateRepLevel();
                CurQuests[i] = null;
                CurQuests[i].QuestID = 0;
                UpdateCurQuests();
            }
        }
    }
}
