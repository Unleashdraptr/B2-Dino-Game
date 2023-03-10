using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteractions : MonoBehaviour, IDataHandler
{
    public float Health;
    public GameObject UI;
    public Transform ItemList;
    bool InShop;

    public Image ItemDescImage;
    public TextMeshProUGUI ItemDescText;

    public void LoadData(GameData data)
    {
        Health = data.Health;
    }
    public void SaveData(ref GameData data)
    {
        data.Health = Health;
    }


    public void BuyItem()
    {
        if(InShop == true)
        {
            Debug.Log("Buy it");
        }
    }
    void CheckHealth()
    {
        if(Health <= 0)
        {
            GetComponent<Movement>().moveState = Movement.MoveState.DEATH;
            UI.GetComponent<UIManager>().DeathScreen.SetActive(true);
        }
    }
    public void OnItemHover(int ItemNum)
    {
        ItemDescText.text = ItemList.GetChild(ItemNum - 1).GetChild(0).GetComponent<ItemID>().Description;
        ItemDescImage.transform.localScale = new(1, 1, 1);
        ItemDescImage = ItemList.GetChild(ItemNum - 1).GetChild(0).GetComponent<Image>();
    }
    public void OnItemLeave(int ItemNum)
    {
        ItemDescText.text = "";
        ItemDescImage.GetComponent<Transform>().localScale = new(0,0,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        InShop = true;
        if (other.gameObject.layer == 6)
        {
            UIManager.Pause = true;
            other.GetComponent<ShopUI>().OpenShopUI();
            other.GetComponent<ShopUI>().OpenFactionUI();
        }
    }
}
