using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is where the Player can interact with the Inventory
public class ItemPickUp : MonoBehaviour
{
    //This is the player's holding position and where it goes when the player drops it
    public Transform ItemPickUpStorage;
    public Transform ItemStorage;
    //This is the pickup prompt to tell the player they can pick it up
    public GameObject PickUpPrompt;
    //Stats that are needed and if its a quest
    public GameObject Stats;
    public QuestStorage quest;
    //The raycast that is used to tell if the player is looking at an item
    public Camera CameraRayCast;

    //The inventory it interacts with
    readonly Inventory inv;

    private void LateUpdate()
    {
        //To stop the player from dropping items when paused
        if (UIManager.Pause == false)
        {
            //It will then shoot a Raycast out of the middle of the screen
            //inv = GetComponent<Inventory>();
            Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(Point, out RaycastHit hit, 5))
            {
                /*
                //If the slot is empty and the player is looking at the item they can have the option to pick it up
                if (hit.transform.CompareTag("Item") && (inv.InventoryItems[inv.CurrentSlot] == false || CompareItem(hit.transform.gameObject)))
                {
                    PickUpPrompt.SetActive(true);
                }
                //Else disable the pickup prompt
                else
                    PickUpPrompt.SetActive(false);
                //If the pickup prompt is active then let them pick it up
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.transform);
                }
                */
                //If its a quest object then also let the player interact with it
                if (CompareItem(hit.transform.gameObject))
                {
                    PickUpPrompt.SetActive(true);
                }
                else
                    PickUpPrompt.SetActive(false);
                //If so then check which quest it was and then finish said quest
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (quest.CurQuests[i].ToSpawn != null)
                        {
                            if (hit.transform.gameObject.CompareTag(quest.CurQuests[i].ToSpawn.tag))
                            {
                                quest.FinishQuest(quest.CurQuests[i].QuestID);
                            }
                        }
                    }
                    PickUpPrompt.SetActive(false);
                }
                /*
                //Else if they have an item in hand then let them drop the item
                else if (inv.InventoryItems[inv.CurrentSlot] == true && Input.GetKeyDown(KeyCode.E))
                {
                    DropItem();
                }
                */
            }
            else
                PickUpPrompt.SetActive(false);
        }
    }
    //This sets the item to the position for the player
    public void PickUpItem(Transform Item)
    {
        if (inv.InventoryItems[inv.CurrentSlot] == false)
        {
            Item.SetParent(ItemPickUpStorage);
            Item.localPosition = new(0, 0, 0);
            inv.InventoryItems[inv.CurrentSlot] = Item.gameObject;
        }
    }
    //This then drops the item at the players eye position
    public void DropItem()
    {
        Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(Point, out RaycastHit hit, 5))
        {
            inv.InventoryItems[inv.CurrentSlot] = null;
            Vector3 Pos = new(hit.point.x, hit.point.y + ItemPickUpStorage.GetChild(0).localScale.y / 2, hit.point.z);
            ItemPickUpStorage.GetChild(0).SetPositionAndRotation(Pos, hit.transform.rotation);
            ItemPickUpStorage.GetChild(0).SetParent(ItemStorage);
        }
    }
    //This is a function mostly used by other scripts. They will check if the item the player currently has in hand
    //can be sold to the stand
    public bool CheckSellItem(int StoreID)
    {
        if (inv.InventoryItems[inv.CurrentSlot])
        {
            Transform Item = ItemPickUpStorage.GetChild(0);
            if (StoreID == Item.GetComponent<ItemID>().itemID)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
    //If the above function is correct, then a popup will appear and this will then be used
    public void SellItem(int FactionNum)
    {
        inv.InventoryItems[inv.CurrentSlot] = null;
        Transform Item = ItemPickUpStorage.GetChild(0);
        StatsStorage stats = Stats.GetComponent<StatsStorage>();
        stats.Currency += Item.GetComponent<ItemID>().sellPrice;
        stats.UpdateMoney();
        stats.RepLevel[FactionNum] += Item.GetComponent<ItemID>().RepValue;
        stats.UpdateRepLevel();
        Destroy(Item.gameObject);
    }
    //This is will be used when looking to see if a plant or an animal is the one that the quest is looking
    public bool CompareItem(GameObject item)
    {
        for (int i = 0; i < quest.QuestCount; i++)
        {
            if (quest.CurQuests[i].questType == ProductID.Quest.ANIMAL || quest.CurQuests[i].questType == ProductID.Quest.PLANT)
            {
                if (item.CompareTag(quest.CurQuests[i].ToSpawn.tag))
                {
                    //Just to make sure it doesnt say untagged items like the ground
                    if (!quest.CurQuests[i].ToSpawn.CompareTag("Untagged"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
