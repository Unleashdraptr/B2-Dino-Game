using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionInfo : MonoBehaviour
{
    public GameObject ShopMenu;
    public GameObject[] ItemsToSell;
    public GameObject ShopPrompt;
    public GameObject SellPrompt;
    public int Faction;
    public int[] LvlLimit = new int[4];

    private void OnTriggerEnter(Collider other)
    {
        ShopUI.InShop = true;
        if (other.gameObject.CompareTag("Player"))
        {
            ShopPrompt.SetActive(true);
            if (other.GetComponent<ItemPickUp>().CheckSellItem(Faction))
            {
                SellPrompt.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameUIManager.Pause = true;
                ShopMenu.GetComponent<ShopUI>().OpenFactionUI(LvlLimit, ItemsToSell, Faction);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && SellPrompt.activeInHierarchy)
            {
                other.GetComponent<ItemPickUp>().SellItem(Faction);
            }
        }
        else
        {
            SellPrompt.SetActive(false);
            ShopPrompt.SetActive(false);
        }
    }
}
