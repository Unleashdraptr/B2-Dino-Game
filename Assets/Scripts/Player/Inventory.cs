using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//This script contains all the information that is needed for items and Inventory
public class Inventory : MonoBehaviour
{
    //This is a list of Gameobject for Saving And Loading, will change this later
    public GameObject[] ItemIDs;
    //This sets up that theres 9 Inventory slots and bools to check which ones are used
    public GameObject[] InventoryItems = new GameObject[8];
    public bool[] UsedSlots = new bool[8];
    //This stores which one the player is on currently
    public int CurrentSlot;
    //Check for if its a weapons
    public bool isHoldingWeapon;
    void Update()
    {
        //Update what slots currently have items
        UpdateUsedSlots();
        //If the player has used the scroll wheel
        if (Input.mouseScrollDelta.y != 0)
        {
            //It will then find the next empty slot and then update to that item
            FindNextEmptySlot(Input.mouseScrollDelta.y);
            UpdateHoldingItem();
        }
        //If its a weapon and they hold down the fire button then itll use the weapon
        if (isHoldingWeapon && Input.GetMouseButton(0))
        {
            InventoryItems[CurrentSlot].GetComponent<WeaponControls>().ActivateWeapons();
        }
    }
    //This is where we find out which slots currently have items inside it
    void UpdateUsedSlots()
    {
        //It loop thorugh the inventory to see if it contains an item and sets the bools to be true if they are
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
        //This then finds the next available empty slots for the player
        int TempSpareSlot = 0;
        //Itll loop through until it finds an empty slot and then set it to true
        for(int i = 0; i < InventoryItems.Length; i++)
        {
            if(!InventoryItems[i])
            {
                TempSpareSlot = i;
            }
        }
        UsedSlots[TempSpareSlot] = true;
    }
    //This is then used to find the next slot when the player uses the scroll wheel
    void FindNextEmptySlot(float Direction)
    {
        int k;
        //If its moving up the inventory
        if (Direction > 0)
        {
            //We loop through all 8 to find the next one that is available
            for (int i = CurrentSlot; i < CurrentSlot + 8; i++)
            {
                if (i > 7)
                {
                    k = i - 8;
                }
                else
                    k = i;
                //Once we find it, we stop there and go to that slot
                if (UsedSlots[k] == true && k != CurrentSlot)
                {
                    CurrentSlot = k;
                    break;
                }
            }
        }
        //Smae thing put for the other direction
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
    //We then run this function to update the item their holding
    void UpdateHoldingItem()
    {
        //We disable all the previous items
        for (int i = 0; i < 8; i++)
        {
            if (InventoryItems[i])
            {
                InventoryItems[i].SetActive(false);
            }
        }
        //We then get the item that is in the current slot and set it to true
        if (InventoryItems[CurrentSlot])
        {
            InventoryItems[CurrentSlot].SetActive(true);
            //Also check if its an item
            if(InventoryItems[CurrentSlot].GetComponent<ItemID>().itemID == 1)
            {
                isHoldingWeapon = true;
            }
            else
                isHoldingWeapon = false;
        }
    }
}
