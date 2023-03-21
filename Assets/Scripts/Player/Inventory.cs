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
    public int CurrentSlot;
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
        if(Input.mouseScrollDelta.y !=0)
        {
            FindNextEmptySlot();
            NextNum((int)Input.mouseScrollDelta.y);
            UpdateHoldingItem();
        }
    }
    void FindNextEmptySlot()
    {
        for(int i = 0; i < 8; i++)
        {
            if(!InventoryItems[i])
            {
                CurrentSlot = i;
                break;
            }
        }
    }
    void NextNum(int Num)
    {
        for(int i = CurrentItem; i<i+8; i+=Num)
        {
            if (!InventoryItems[i])
            {
                CurrentItem = CurrentSlot;
                break;
            }
            else if (InventoryItems[i] || i == CurrentSlot)
            {
                if (i > 8)
                {
                    CurrentItem = (i - 8);
                }
                else if(i < 0)
                {
                    CurrentItem = (i + 8);
                } 
                else
                    CurrentItem = i;
                break;
            }
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
