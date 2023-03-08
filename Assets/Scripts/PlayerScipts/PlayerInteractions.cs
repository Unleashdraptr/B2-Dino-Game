using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour, IDataHandler
{
    public float Health;
    public GameObject UI;

    public void LoadData(GameData data)
    {
        Health = data.Health;
    }
    public void SaveData(ref GameData data)
    {
        data.Health = Health;
    }


    void CheckHealth()
    {
        if(Health <= 0)
        {
            GetComponent<Movement>().moveState = Movement.MoveState.DEATH;
            UI.GetComponent<UIManager>().DeathScreen.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && (other.gameObject.CompareTag("Conservation") || other.gameObject.CompareTag("Biologists") || other.gameObject.CompareTag("Hunters")))
        {
            UIManager.Pause = true;
            other.GetComponent<ShopUI>().OpenShopUI();
            switch(other.gameObject.tag)
            {
                case "Conservation":
                    other.GetComponent<ShopUI>().OpenConservUI();
                    break;
                case "Hunters":
                    other.GetComponent<ShopUI>().OpenHunterUI();
                    break;
                case "Biologists":
                    other.GetComponent<ShopUI>().OpenBiologUI();
                    break;
            }
        }
    }
}
