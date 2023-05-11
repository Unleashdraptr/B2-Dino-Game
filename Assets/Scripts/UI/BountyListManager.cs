using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BountyListManager : MonoBehaviour
{
    public QuestSetup Quest;
    public StatsStorage Stats;
    public Transform BountySlots;
    public QuestStorage Queststorage;
    int PageNum;
    int BountNum;

    public void OpenBountyList(int LvlLimit)
    {
        int SlotsFilled = 0;
        ShopUI.InShop = true;
        transform.GetChild(0).gameObject.SetActive(true);
        PageNum = 1;
        BountNum = 5 * (PageNum - 1);
        for(int i = 0; i < Quest.Quests.Length; i++)
        {
            if(Quest.Quests[i].questType == ProductID.Quest.BOUNTY && i - BountNum <  6)
            {
                if (Quest.Quests[BountNum + SlotsFilled].Dangerlevel <= LvlLimit)
                {
                    Transform CurBount = BountySlots.GetChild(i);
                    CurBount.GetChild(0).gameObject.SetActive(true);
                    CurBount.GetChild(1).gameObject.SetActive(true);
                    CurBount.GetChild(0).GetComponent<Image>().sprite = Quest.Quests[BountNum + SlotsFilled].PreviewImage;
                    if (Quest.Quests[BountNum + SlotsFilled].Cost <= Stats.Currency)
                    {
                        CurBount.GetComponent<Button>().interactable = true;
                    }
                    else
                        CurBount.GetComponent<Button>().interactable = false;
                    SlotsFilled++;
                }
            }
            if (i + 1 >= Quest.Quests.Length || SlotsFilled == 6)
            {
                break;
            }
        }
    }
    public void BuyBounty(int ItemNum)
    {
        if (BountySlots.GetChild(1).gameObject.activeInHierarchy)
        {
            int DataNum = ItemNum + (5 * (PageNum - 1));
            Quest.BountySetup(Quest.Quests[DataNum].QuestID);
            Stats.UpdateMoney();
        }
    }
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
