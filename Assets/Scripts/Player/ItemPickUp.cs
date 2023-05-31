using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Transform ItemPickUpStorage;
    public Transform ItemStorage;
    public GameObject PickUpPrompt;
    public GameObject Stats;
    public Camera CameraRayCast;
    public QuestStorage quest;
    Inventory inv;

    private void LateUpdate()
    {
        if (UIManager.Pause == false)
        {
            //inv = GetComponent<Inventory>();
            Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(Point, out RaycastHit hit, 5))
            {
                /*
                if (hit.transform.CompareTag("Item") && (inv.InventoryItems[inv.CurrentSlot] == false || CompareItem(hit.transform.gameObject)))
                {
                    PickUpPrompt.SetActive(true);
                }
                else
                    PickUpPrompt.SetActive(false);
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.transform);
                }
                */
                Debug.Log(hit.transform.gameObject);
                if (CompareItem(hit.transform.gameObject))
                {
                    PickUpPrompt.SetActive(true);
                }
                else
                    PickUpPrompt.SetActive(false);
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (quest.CurQuests[i].ToSpawn != null)
                        {
                            if (quest.CurQuests[i].ToSpawn.tag == hit.transform.gameObject.tag)
                            {
                                quest.FinishQuest(quest.CurQuests[i].QuestID);
                            }
                        }
                    }
                    PickUpPrompt.SetActive(false);
                }
                /*
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
    public void PickUpItem(Transform Item)
    {
        if (inv.InventoryItems[inv.CurrentSlot] == false)
        {
            Item.SetParent(ItemPickUpStorage);
            Item.localPosition = new(0, 0, 0);
            inv.InventoryItems[inv.CurrentSlot] = Item.gameObject;
        }
    }
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

    public bool CompareItem(GameObject item)
    {
        for (int i = 0; i < quest.QuestCount; i++)
        {
            if (quest.CurQuests[i].questType == ProductID.Quest.ANIMAL || quest.CurQuests[i].questType == ProductID.Quest.PLANT)
            {
                if (quest.CurQuests[i].ToSpawn.tag == item.tag)
                {
                    Debug.Log(item.tag);
                    Debug.Log(quest.CurQuests[i].ToSpawn.tag);
                    if (quest.CurQuests[i].ToSpawn.tag != "Untagged")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
