using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour, IDataHandler
{
    public float Health;
    public int Wealth;
    public GameObject UI;

    public void LoadData(GameData data)
    {
        Health = data.Health;
        Wealth = data.Wealth;
    }
    public void SaveData(ref GameData data)
    {
        data.Health = Health;
        data.Wealth = Wealth;
    }


    void CheckHealth()
    {
        if(Health <= 0)
        {
            GetComponent<Movement>().moveState = Movement.MoveState.DEATH;
            UI.GetComponent<UIManager>().DeathScreen.SetActive(true);
        }
    }
}
