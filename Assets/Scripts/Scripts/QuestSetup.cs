using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class QuestSetup : MonoBehaviour
{
    public QuestStorage Queststorage;
    public StatsStorage Stats;
    public Transform WaypointStorage;
    public GameObject AreaDetection;

    public QuestsList[] Quests;

    public void BountySetup(int QuestID)
    {
        if (Queststorage.QuestLimit == false)
        {
            if (FindQuest(QuestID) != null)
            {
                QuestsList CurQuest = FindQuest(QuestID);
                Stats.Currency -= CurQuest.Cost;
                Queststorage.AddQuest(CurQuest);
                CurQuest.Taken = true;
                Queststorage.UpdateCurQuests();
                GameObject Animal = Instantiate(CurQuest.ToSpawn, CurQuest.SpawnPos, Quaternion.identity, GameObject.Find("AnimalsStorage").transform);
                Animal.GetComponent<IsTargetAnimal>().enabled = true;
            }
        }
    }
    public void AreaSelection(int QuestID)
    {      
        if (Queststorage.QuestLimit == false)
        {
            Debug.Log(QuestID);
            if (FindQuest(QuestID) != null)
            {
                Debug.Log("haternaesmstk,d8u");
                QuestsList CurQuest = FindQuest(QuestID);
                Stats.Currency -= CurQuest.Cost;
                Queststorage.AddQuest(CurQuest);
                CurQuest.Taken = true;
                Queststorage.UpdateCurQuests();
                Vector3 Pos = new(CurQuest.SpawnPos.x, 1000, CurQuest.SpawnPos.z);
                Instantiate(AreaDetection, Pos, Quaternion.identity, WaypointStorage);
            }
        }
    }
    public void ItemSelection(int QuestID)
    {
        if (Queststorage.QuestLimit == false)
        {
            if (FindQuest(QuestID) != null)
            {
                QuestsList CurQuest = FindQuest(QuestID);
                Stats.Currency -= CurQuest.Cost;
                Queststorage.AddQuest(CurQuest);
                CurQuest.Taken = true;
                Queststorage.UpdateCurQuests();

            }
        }
    }
    public void SpeciesSelection(bool isPlant, int QuestID)
    {
        if (Queststorage.QuestLimit == false)
        {
            if (FindQuest(QuestID) != null)
            {
                QuestsList CurQuest = FindQuest(QuestID);
                Stats.Currency -= CurQuest.Cost;
                Queststorage.AddQuest(CurQuest);
                CurQuest.Taken = true;
                Queststorage.UpdateCurQuests();
                if(isPlant)
                {

                }
                else
                {

                }

            }
        }
    }



    public QuestsList FindQuest(int QuestID)
    {
        for (int i = 0; i < Quests.Length; i++)
        {
            if (Quests[i].QuestID == QuestID)
            {
                return Quests[i];
            }
        }
        return null;
    }


    float CalculateHeight(float x, float z)
    {
        Vector3 Pos = new(x, 1000, z);
        float y = 0;
        if (Physics.Raycast(Pos, Vector3.down, out RaycastHit hit, 1000))
        {
            y = 1000 - hit.distance;
        }
        return y;
    }
}




[System.Serializable]
public class QuestsList
{
    public string QuestName;
    public ProductID.Quest questType;
    public Vector3 SpawnPos;
    public int Faction;

    public int Cost;
    public int Reward;
    public int FactionReward;
    public string Desc;

    public bool Taken;
    public bool Completed;

    public Sprite PreviewImage;
    public GameObject ToSpawn;
    [Range(0, 5)]
    public int Dangerlevel;
    public int QuestID;
}

