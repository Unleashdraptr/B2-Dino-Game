using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLibrary : MonoBehaviour
{
    public GameObject WaypointLocation;
    public GameObject Player;
    public QuestStorage Queststorage;
    public StatsStorage Stats;

    public void BountySetup(Bounty Bounties, Transform BountySlots)
    {
        if (Queststorage.QuestLimit == false)
        {
            if (BountySlots.GetChild(1).gameObject.activeInHierarchy)
            {
                Stats.Currency -= Bounties.Cost;
                Queststorage.CurBountys[0] = Bounties;
                Queststorage.UpdateCurQuests();
                GameObject Animal = Instantiate(Bounties.Animal, Bounties.SpawnPos, Quaternion.identity, GameObject.Find("AnimalsStorage").transform);
                Animal.GetComponent<IsTargetAnimal>().enabled = true;
            }
        }
    }
    public void AreaSelection(Transform AreaCentre)
    {

    }

    public void ItemSelection()
    {

    }

    public void SpeciesSelection()
    {

    }
}
