using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static bool InShop;
    public StatsStorage Stats;
    public Transform ItemDesc;
    public QuestSetup quests;

    public void OpenFactionUI(int[] LvlLimit, GameObject[] ItemsToSell, int Faction)
    {
        InShop = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        int ShopNum = 0;
        for (int j = 0; j < Stats.RepLevel[Faction]; j++)
        {
            for (int i = 0; i < LvlLimit[j] + 5; i++)
            {
                GameObject Item = Instantiate(ItemsToSell[ShopNum], transform.GetChild(1).GetChild(ShopNum));
                if (Item.GetComponent<ProductID>().Cost <= Stats.Currency)
                {
                    transform.GetChild(1).GetChild(ShopNum).GetComponent<Button>().interactable = true;
                }
                ShopNum++;
            }
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BuyItem(int ItemNum)
    {
        ProductID Item = transform.GetChild(1).GetChild(ItemNum - 1).GetComponent<ProductID>();
        if (Item.IsQuest == true && Item.ItemID == 0)
        {
            ActivateQuest(Item);
        }
        else
        {

        }
        Stats.UpdateMoney();
    }
    public void OnItemHover(int ItemNum)
    {
        if (transform.GetChild(1).GetChild(ItemNum - 1).childCount != 0)
        {
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.GetChild(1).GetChild(ItemNum - 1).GetChild(0).GetComponent<ProductID>().Description;
            ItemDesc.GetChild(2).gameObject.SetActive(true);
            ItemDesc.GetChild(2).GetComponent<Image>().sprite = transform.GetChild(1).GetChild(ItemNum - 1).GetChild(0).GetComponent<Image>().sprite;
        }
    }
    public void OnItemLeave()
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        ItemDesc.GetChild(2).gameObject.SetActive(false);
    }
    void ActivateQuest(ProductID Item)
    {
        switch (Item.quest)
        {
            case ProductID.Quest.ANIMAL:
                quests.SpeciesSelection(false, Item.QuestNum);
                break;
            case ProductID.Quest.PLANT:
                quests.SpeciesSelection(true, Item.QuestNum);
                break;
            case ProductID.Quest.AREA:
                quests.AreaSelection(Item.QuestNum);
                break;
            case ProductID.Quest.ITEM:
                quests.ItemSelection(Item.QuestNum);
                break;
        }
    }
}
