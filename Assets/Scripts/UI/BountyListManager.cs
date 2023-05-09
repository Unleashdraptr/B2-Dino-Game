using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BountyListManager : MonoBehaviour
{
    public QuestLibrary bountybuy;
    public StatsStorage Stats;
    public Transform BountySlots;
    public QuestStorage Queststorage;
    public Bounty[] Bounties;
    int PageNum;
    int BountNum;

    public void OpenBountyList(int LvlLimit)
    {
        ShopUI.InShop = true;
        transform.GetChild(0).gameObject.SetActive(true);
        PageNum = 1;
        BountNum = 5 * (PageNum - 1);
        for (int i = 0; i < BountySlots.childCount; i++)
        {
            if (Bounties[BountNum + i].Dangerlevel <= LvlLimit)
            {
                Transform CurBount = BountySlots.GetChild(i);
                CurBount.GetChild(0).gameObject.SetActive(true);
                CurBount.GetChild(1).gameObject.SetActive(true);
                CurBount.GetChild(0).GetComponent<Image>().sprite = Bounties[BountNum + i].AnimalImage;
                if(Bounties[BountNum + i].Cost <= Stats.Currency)
                {
                    CurBount.GetComponent<Button>().interactable = true;
                }
                else
                    CurBount.GetComponent<Button>().interactable = false;
            }
            if (i + 1 >= Bounties.Length)
            {
                break;
            }
        }
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ShopUI.InShop == true)
        {
            ShopUI.InShop = false;
            transform.GetChild(0).gameObject.SetActive(false);
            UIManager.Pause = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void BuyBounty(int ItemNum)
    {
        int DataNum = ItemNum + (5 * (PageNum - 1));
        bountybuy.BountySetup(Bounties[DataNum], BountySlots.GetChild(ItemNum));
        Bounties[DataNum] = null;
    }
    public void OnItemHover(int ItemNum)
    {
        if (BountySlots.GetChild(ItemNum).GetChild(1).gameObject.activeInHierarchy)
        {
            int DataNum = ItemNum + (5 * (PageNum - 1));
            Animator Animations = BountySlots.GetChild(ItemNum).GetComponent<Animator>();
            BountySlots.GetChild(ItemNum).GetChild(2).GetChild(1).gameObject.SetActive(true);
            BountySlots.GetChild(ItemNum).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Bounties[DataNum].Cost.ToString();
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
[System.Serializable]
public class Bounty
{
    public Sprite AnimalImage;
    public GameObject Animal;
    public Vector3 SpawnPos;
    [Range(0,5)]
    public int Dangerlevel;
    public int Cost;
    public int Reward;
    public string AnimalDesc;
}

