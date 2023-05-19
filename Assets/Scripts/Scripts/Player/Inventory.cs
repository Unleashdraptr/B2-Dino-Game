using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] ItemIDs;
    public GameObject[] InventoryItems = new GameObject[8];
    public bool[] UsedSlots = new bool[8];
    public int CurrentSlot;
    public bool isHoldingWeapon;
    // Update is called once per frame
    void Update()
    {
        UpdateUsedSlots();
        if (Input.mouseScrollDelta.y != 0)
        {
            FindNextEmptySlot(Input.mouseScrollDelta.y);
            UpdateHoldingItem();
        }
        if (isHoldingWeapon && Input.GetMouseButton(0))
        {
            InventoryItems[CurrentSlot].GetComponent<WeaponControls>().ActivateWeapons();
        }
    }
    void UpdateUsedSlots()
    {
        for(int i = 0; i < InventoryItems.Length; i++)
        {
            if(InventoryItems[i])
            {
                UsedSlots[i] = true;
            }
            if (!InventoryItems[i])
            {
                UsedSlots[i] = false;
            }
        }
        int TempSpareSlot = 0;
        for(int i = 0; i < InventoryItems.Length; i++)
        {
            if(!InventoryItems[i])
            {
                TempSpareSlot = i;
            }
        }
        UsedSlots[TempSpareSlot] = true;
    }
    void FindNextEmptySlot(float Direction)
    {
        int k;
        if (Direction > 0)
        {
            for (int i = CurrentSlot; i < CurrentSlot + 8; i++)
            {
                if (i > 7)
                {
                    k = i - 8;
                }
                else
                    k = i;
                if (UsedSlots[k] == true && k != CurrentSlot)
                {
                    CurrentSlot = k;
                    break;
                }
            }
        }
        if (Direction < 0)
        {
            for (int i = CurrentSlot; i > CurrentSlot - 8; i--)
            {
                if (i < 0)
                {
                    k = i + 8;
                }
                else
                    k = i;
                if (UsedSlots[k] == true && k != CurrentSlot)
                {
                    CurrentSlot = k;
                    break;
                }
            }
        }
    }
    void UpdateHoldingItem()
    {
        for (int i = 0; i < 8; i++)
        {
            if (InventoryItems[i])
            {
                InventoryItems[i].SetActive(false);
            }
        }
        if (InventoryItems[CurrentSlot])
        {
            InventoryItems[CurrentSlot].SetActive(true);
            if(InventoryItems[CurrentSlot].GetComponent<ItemID>().itemID == 1)
            {
                isHoldingWeapon = true;
            }
            else
                isHoldingWeapon = false;
        }
    }
}
