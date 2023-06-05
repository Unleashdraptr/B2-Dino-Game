using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BountyListManager : MonoBehaviour
{
    //This gets the Quests and the stats to make sure the player can get the quest
    public QuestSetup Quest;
    public StatsStorage Stats;
    public QuestStorage Queststorage;
    //This is then used to set up the shop
    public Transform BountySlots;
    int PageNum;
    int BountNum;

    //Once opening the shop itll take into account the LvlLimit
    public void OpenBountyList(int LvlLimit)
    {
        int SlotsFilled = 0;
        //Set active it and pause the game
        ShopUI.InShop = true;
        transform.GetChild(0).gameObject.SetActive(true);
        //We then set it to page 1 (0 in the code)
        PageNum = 1;
        BountNum = 5 * (PageNum - 1);
        //We then run a for loop to spawn as many quests as we can for our Level
        for(int i = 0; i < Quest.Quests.Length; i++)
        {
            //If it doesnt fill the screen and is a bounty
            if(Quest.Quests[i].questType == ProductID.Quest.BOUNTY && i - BountNum <  6)
            {
                //If the player is the right level for it
                if (Quest.Quests[BountNum + SlotsFilled].Dangerlevel <= LvlLimit)
                {
                    //We then set up the bounty in the UI and let the player interact with it
                    Transform CurBount = BountySlots.GetChild(i);
                    CurBount.GetChild(0).gameObject.SetActive(true);
                    CurBount.GetChild(1).gameObject.SetActive(true);
                    CurBount.GetChild(0).GetComponent<RawImage>().texture = Quest.Quests[BountNum + SlotsFilled].PreviewImage;
                    if (Quest.Quests[BountNum + SlotsFilled].Cost <= Stats.Currency)
                    {
                        CurBount.GetComponent<Button>().interactable = true;
                    }
                    else
                        CurBount.GetComponent<Button>().interactable = false;
                    SlotsFilled++;
                }
            }
            //If we fill the screen then we break
            if (i + 1 >= Quest.Quests.Length || SlotsFilled == 6)
            {
                break;
            }
        }
    }
    //If the player decides the want the bounty then this is run
    public void BuyBounty(int ItemNum)
    {
        if (BountySlots.GetChild(1).gameObject.activeInHierarchy)
        {
            int DataNum = ItemNum + (5 * (PageNum - 1));
            Quest.BountySetup(Quest.Quests[DataNum].QuestID);
            Stats.UpdateMoney();
        }
    }
    //This will tell the player what the price is and more information
    public void OnItemHover(int ItemNum)
    {
        if (BountySlots.GetChild(ItemNum).GetChild(1).gameObject.activeInHierarchy)
        {
            int DataNum = ItemNum + (5 * (PageNum - 1));
            Animator Animations = BountySlots.GetChild(ItemNum).GetComponent<Animator>();
            BountySlots.GetChild(ItemNum).GetChild(2).GetChild(1).gameObject.SetActive(true);
            BountySlots.GetChild(ItemNum).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Quest.Quests[DataNum].Cost.ToString();
            Animations.SetBool("IsHoveredOn", true);
        }
    }
    //This will remove the before mentioned affects
    public void OffItemHover(int ItemNum)
    {
        if (BountySlots.GetChild(ItemNum).GetChild(1).gameObject.activeInHierarchy)
        {
            Animator Animations = BountySlots.GetChild(ItemNum).GetComponent<Animator>();
            BountySlots.GetChild(ItemNum).GetChild(2).GetChild(1).gameObject.SetActive(false);
            Animations.SetBool("IsHoveredOn", false);
        }
    }
}
