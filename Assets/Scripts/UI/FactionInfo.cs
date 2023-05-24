using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionInfo : MonoBehaviour
{
    public DialogueManager Dialogue;
    public GameObject[] ItemsToSell;
    public GameObject ShopPrompt;
    public GameObject SellPrompt;
    public GameObject Player;
    public int Faction;
    public int[] LvlLimit = new int[4];
    bool WithinTrigger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && WithinTrigger)
        {
            SellPrompt.SetActive(false);
            ShopPrompt.SetActive(false);
            Dialogue.StartShopDialogue(transform.GetChild(0).gameObject, this);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && SellPrompt.activeInHierarchy)
        {
            Player.GetComponent<ItemPickUp>().SellItem(Faction);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
        else
        {
            SellPrompt.SetActive(false);
            ShopPrompt.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        WithinTrigger = false;
        SellPrompt.SetActive(false);
        ShopPrompt.SetActive(false);
    }
}
