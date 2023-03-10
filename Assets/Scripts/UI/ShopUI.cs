using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static bool InShop;
    public GameObject ShopMenu;
    public GameObject RepLevels;
    public GameObject[] ItemsToSell;
    public int[] LvlLimit = new int[4];

    public void OpenShopUI()
    {
        InShop = true;
        ShopMenu.SetActive(true);
    }

    public void OpenFactionUI()
    {
        int ShopNum = 0;
        for (int j = 0; j < RepLevels.GetComponent<StatsStorage>().RepLevel[0]; j++)
        {
            if (j == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject Item = Instantiate(ItemsToSell[ShopNum], ShopMenu.transform.GetChild(0).GetChild(1).GetChild(ShopNum));
                    ShopMenu.transform.GetChild(0).GetChild(1).GetChild(ShopNum).GetComponent<Button>().interactable = true;
                    Item.GetComponent<ItemID>().HasItem = true;
                    ShopNum++;
                }
            }
            else
            {
                for (int i = 0; i < LvlLimit[j]; i++)
                {
                    GameObject Item = Instantiate(ItemsToSell[ShopNum], ShopMenu.transform.GetChild(0).GetChild(1).GetChild(ShopNum));
                    ShopMenu.transform.GetChild(0).GetChild(1).GetChild(ShopNum).GetComponent<Button>().interactable = true;
                    Item.GetComponent<ItemID>().HasItem = true;
                    ShopNum++;
                }
            }
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
