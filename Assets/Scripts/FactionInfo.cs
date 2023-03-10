using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionInfo : MonoBehaviour
{
    public GameObject ShopMenu;
    public GameObject[] ItemsToSell;
    public int Faction;
    public int[] LvlLimit = new int[4];
    private void OnTriggerEnter(Collider other)
    {
        ShopUI.InShop = true;
        if (other.gameObject.CompareTag("Player"))
        {
            GameUIManager.Pause = true;
            ShopMenu.GetComponent<ShopUI>().OpenFactionUI(LvlLimit, ItemsToSell, Faction);
        }
    }
}
