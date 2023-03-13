using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataHandler
{
    public GameObject[] ItemIDs;
    public GameObject[] InventoryItems = new GameObject[8];
    public int CurrentItem;
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < 8; i++)
        {
            if (InventoryItems[i])
            {
                data.HasItem[i] = true;
                data.ItemPrefabID[i] = InventoryItems[i].GetComponent<ItemID>().PublicitemID;
                data.FactionID[i] = InventoryItems[i].GetComponent<ItemID>().itemID;
                data.itemName[i] = InventoryItems[i].GetComponent<ItemID>().itemName;
                data.sellPrice[i] = InventoryItems[i].GetComponent<ItemID>().sellPrice;
                data.RepValue[i] = InventoryItems[i].GetComponent<ItemID>().RepValue;
            }
        }
    }
    public void LoadData(GameData data)
    {
        for (int i = 0; i < 8; i++)
        {
            if (data.HasItem[i])
            {
                GameObject item = Instantiate(ItemIDs[data.ItemPrefabID[i]], transform.GetChild(0));
                InventoryItems[i] = item;
                item.GetComponent<ItemID>().itemID = data.FactionID[i];
                item.GetComponent<ItemID>().itemName = data.itemName[i];
                item.GetComponent<ItemID>().sellPrice = data.sellPrice[i];
                item.GetComponent<ItemID>().RepValue = data.RepValue[i];
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CurrentItem += (int)Input.mouseScrollDelta.y;
        if(CurrentItem > 7)
        {
            CurrentItem = 0;
        }
        else if(CurrentItem < 0)
        {
            CurrentItem = 7;
        }
        if(Input.mouseScrollDelta.y !=0)
        {
            UpdateHoldingItem();
        }
    }
    void UpdateHoldingItem()
    {
        for (int i = 0; i < 8; i++)
        {
            if (InventoryItems[i])
            {
                if (i == CurrentItem)
                {
                    InventoryItems[i].SetActive(true);
                }
                else
                    InventoryItems[i].SetActive(false);
            }
        }
    }
}
