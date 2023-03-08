using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void OpenHunterUI()
    {
        int ShopNum = 0;
        for (int j = 0; j < RepLevels.GetComponent<StatsStorage>().RepLevel[0]; j++)
        {
            Debug.Log("j: " + j);
            for (int i = 0; i < LvlLimit[j]; i++)
            {
                Instantiate(ItemsToSell[ShopNum], ShopMenu.transform.GetChild(0).GetChild(1).GetChild(ShopNum));
                Debug.Log("i: " + i);
                ShopNum++;
            }
        }
    }
    public void OpenBiologUI()
    {

    }
    public void OpenConservUI()
    {

    }
}
