using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//This is the script that controls the shops UI 
public class ShopUI : MonoBehaviour
{
    //This bools acts like another pause, just for the shops 
    public static bool InShop;
    //This is the stats and Quests they pull from
    public StatsStorage Stats;
    public QuestSetup quests;
    //This is how more information is displayed 
    public Transform ItemDesc;

    //This will then open the ShopUI form ShopStock and load up what is needed
    public void OpenShopUI(int[] LvlLimit, GameObject[] ItemsToSell, int Faction)
    {
        InShop = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        int ShopNum = 0;
        //It will loop through the Reputation the player has with the Faction and then loop through the items it 
        //can place because of that
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
    //When the player buys an item, itll check if its a quest, if so then add it to the list, the items are currently
    //not implemented
    public void BuyItem(int ItemNum)
    {
        ProductID Item = transform.GetChild(1).GetChild(ItemNum).GetComponentInChildren<ProductID>();
        if (Item.IsQuest == true && Item.ItemID == 0)
        {
            ActivateQuest(Item);
        }
        else
        {

        }
        Stats.UpdateMoney();
    }
    //This will display the information when hovered over
    public void OnItemHover(int ItemNum)
    {
        if (transform.GetChild(1).GetChild(ItemNum).childCount != 0)
        {
            ItemDesc.GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.GetChild(1).GetChild(ItemNum).GetComponentInChildren<ProductID>().Description;
            ItemDesc.GetChild(2).gameObject.SetActive(true);
            ItemDesc.GetChild(2).GetComponent<Image>().sprite = transform.GetChild(1).GetChild(ItemNum).GetComponentInChildren<ProductID>().PreviewImage;
        }
    }
    //This will remove the info when leaving the box
    public void OnItemLeave()
    {
        ItemDesc.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        ItemDesc.GetChild(2).gameObject.SetActive(false);
    }
    //This will then activate the quest and use different functions based of its type
    void ActivateQuest(ProductID Item)
    {
        switch (Item.quest)
        {
            case ProductID.Quest.ANIMAL:
                quests.SpeciesSelection(false, Item.QuestID);
                break;
            case ProductID.Quest.PLANT:
                quests.SpeciesSelection(true, Item.QuestID);
                break;
            case ProductID.Quest.AREA:
                quests.AreaSelection(Item.QuestID);
                break;
            case ProductID.Quest.ITEM:
                quests.ItemSelection(Item.QuestID);
                break;
        }
    }
}
