using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//This is used to setup the Quests that the player buys
public class QuestSetup : MonoBehaviour
{
    //It gets the Stats and where to store the Quests
    public QuestStorage Queststorage;
    public StatsStorage Stats;
    //It then gets where to store the objects
    public Transform WaypointStorage;
    public GameObject AreaDetection;

    //These are all the quests that are in the game
    public QuestsList[] Quests;

    //This is how the Bounty is setup
    public void BountySetup(int QuestID)
    {
        //We check if its in the limit
        if (Queststorage.QuestLimit == false)
        {
            //We then find the quest
            if (FindQuest(QuestID) != null)
            {
                //We then take the money and spawn the animal in its 'boss' mode
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
        //We check if its in the limit
        if (Queststorage.QuestLimit == false)
        {
            //We then find the quest
            if (FindQuest(QuestID) != null)
            {
                //We then take the money and spawn the waypoint to tell the player where to go
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
        //We check if its in the limit
        if (Queststorage.QuestLimit == false)
        {
            //We then find the quest
            if (FindQuest(QuestID) != null)
            {
                //We then take the money and spawn the item for the player to find
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
        //We check if its in the limit
        if (Queststorage.QuestLimit == false)
        {
            //We then find the quest
            if (FindQuest(QuestID) != null)
            {
                //We then take the money and let the player go find the plant and or animal they need to find
                QuestsList CurQuest = FindQuest(QuestID);
                Stats.Currency -= CurQuest.Cost;
                Queststorage.AddQuest(CurQuest);
                CurQuest.Taken = true;
                Queststorage.UpdateCurQuests();
            }
        }
    }
    //This is used by many scripts to get information about a certain Quest
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
}



//This is the data format that the quests are then stored in, not all quests use all the variables
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

    public Texture PreviewImage;
    public GameObject ToSpawn;
    [Range(0, 5)]
    public int Dangerlevel;
    public int QuestID;
}

