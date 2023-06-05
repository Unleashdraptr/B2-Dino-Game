using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This controls the Shops
public class ShopStock : MonoBehaviour
{
    //What is used to tell if the player is in range
    public GameObject ShopPrompt;
    public GameObject SellPrompt;
    public GameObject Player;
    bool WithinTrigger;
    //The shopkeepers Dialogue option
    public DialogueManager Dialogue;
    //The store for its items and what faction it is
    public GameObject[] ItemsToSell;
    public int Faction;
    //The LvlLimit to display each set of items at
    public int[] LvlLimit = new int[4];
    private void Update()
    {
        //If the player is in range then start the shop dialogue
        if (Input.GetKeyDown(KeyCode.E) && WithinTrigger)
        {
            SellPrompt.SetActive(false);
            ShopPrompt.SetActive(false);
            Dialogue.StartShopDialogue(transform.GetChild(0).gameObject, this);
        }
        //If the player has an item it can sell then sell it
        else if (Input.GetKeyDown(KeyCode.Q) && SellPrompt.activeInHierarchy)
        {
            Player.GetComponent<ItemPickUp>().SellItem(Faction);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //This will check if the player is close to the shop and let them interact with it if the are
        WithinTrigger = true;
        if (other.gameObject.CompareTag("Player"))
        {
            ShopPrompt.SetActive(true);
            /*
            if (other.GetComponent<ItemPickUp>().CheckSellItem(Faction))
            {
                SellPrompt.SetActive(true);
            }
            */
        }
        //Else dont
        else
        {
            SellPrompt.SetActive(false);
            ShopPrompt.SetActive(false);
        }
    }
    //Once they go out of range then dont let them interact with it 
    private void OnTriggerExit(Collider other)
    {
        WithinTrigger = false;
        SellPrompt.SetActive(false);
        ShopPrompt.SetActive(false);
    }
}
