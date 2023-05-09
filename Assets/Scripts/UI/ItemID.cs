using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID : MonoBehaviour
{
    //Name that will show in the UI
    public string itemName;
    //What Faction the Item belongs to and what shop it needs to be taken to
    public int itemID;
    //How much the item will sell for at its base price
    public int sellPrice;
    //How much its worth in RepValue
    public int RepValue;
}
