using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Transform ItemPickUpStorage;
    public Transform WorldItems;
    public GameObject PickUpPrompt;
    public GameObject Stats;
    public Camera CameraRayCast;
    Inventory inv;

    private void LateUpdate()
    {
        if (GameUIManager.Pause == false)
        {
            inv = GetComponent<Inventory>();
            Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(Point, out RaycastHit hit, 5))
            {
                if (hit.transform.CompareTag("Item") && inv.InventoryItems[inv.CurrentSlot] == false)
                {
                    PickUpPrompt.SetActive(true);
                }
                else
                    PickUpPrompt.SetActive(false);
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.transform);
                }
                else if (inv.InventoryItems[inv.CurrentSlot] == true && Input.GetKeyDown(KeyCode.E))
                {
                    DropItem();
                }
            }
        }
    }
    public void PickUpItem(Transform Item)
    {
        if(inv.InventoryItems[inv.CurrentSlot] == false)
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
            ItemPickUpStorage.GetChild(0).SetParent(WorldItems);
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
        stats.SetRepLevel(FactionNum);
        Destroy(Item.gameObject);
    }
}
