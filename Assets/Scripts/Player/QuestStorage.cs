using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    public StatsStorage stats;
    public int QuestCount;
    public bool QuestLimit;
    public QuestsList[] CurQuests = new QuestsList[3];
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurQuests();
    }
    // Update is called once per frame
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
        if (QuestCount == 3)
        {
            QuestLimit = true;
        }
        else
            QuestLimit = false;
    }
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
            else if (FoundQuest == true)
            {
                CurQuests[i - 1] = CurQuests[i];
            }
        }
        UpdateCurQuests();
    }

    public void FinishQuest(int QuestID)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CurQuests[i].QuestID == QuestID)
            {
                stats.Currency += CurQuests[i].Reward;
                stats.RepLevel[CurQuests[i].Faction] += CurQuests[i].FactionReward;
                CurQuests[i] = null;
                UpdateCurQuests();
            }
        }
    }
}
