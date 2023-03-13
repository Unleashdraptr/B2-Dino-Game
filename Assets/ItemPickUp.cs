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
    public bool holdingItem;

    private void Update()
    {
        Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width/2, Screen.height/2,0));
        if (Physics.Raycast(Point, out RaycastHit hit, 5))
        {
            PickUpPrompt.SetActive(true);
            if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
            {
                if (holdingItem == false && hit.transform.CompareTag("Item"))
                {
                    PickUpItem(hit.transform);
                }
                else if (holdingItem == true)
                {
                    DropItem();
                }
            }
        }
        else
            PickUpPrompt.SetActive(false);
    }
    public void PickUpItem(Transform Item)
    {
        if(Item.GetComponent<ItemID>().isPickUpable)
        {
            Item.SetParent(ItemPickUpStorage);
            Item.localPosition = new(0, 0, 0);
            holdingItem = true;
        }
    }
    public void DropItem()
    {
        Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(Point, out RaycastHit hit, 5))
        {
            Vector3 Pos = new(hit.point.x, hit.point.y + ItemPickUpStorage.GetChild(0).localScale.y / 2, hit.point.z);
            ItemPickUpStorage.GetChild(0).SetPositionAndRotation(Pos, hit.transform.rotation);
            ItemPickUpStorage.GetChild(0).SetParent(WorldItems);
            holdingItem = false;
        }
    }
    public bool CheckSellItem(int StoreID)
    {
        if (holdingItem == true)
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
        Transform Item = ItemPickUpStorage.GetChild(0);
        Stats.GetComponent<StatsStorage>().Currency += Item.GetComponent<ItemID>().sellPrice;
        Stats.GetComponent<StatsStorage>().RepLevel[FactionNum] += Item.GetComponent<ItemID>().RepValue;
        Destroy(Item);
    }
}
