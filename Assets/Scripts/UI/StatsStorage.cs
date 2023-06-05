using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//This is where the players in game stats are stored and then updated in the UI
public class StatsStorage : MonoBehaviour, IDataHandler
{
    //Hunters, Biologists, Explorers. there reputation number and the current level
    public int[] Reputation = new int[3];
    public int[] RepLevel = new int[3];
    //Their gold and the UI that relates to it
    public int Currency;
    public TextMeshProUGUI GoldNum;
    //This is all saved to their Savefile
    public void SaveData(ref GameData data)
    {
        //Stores the numbers in the file
        for (int i = 0; i < 3; i++)
        {
            data.Reputations[i] = Reputation[i];
        }
        data.Currency = Currency;
        Currency = data.Currency;
    }
    //And then loaded from said file
    public void LoadData(GameData data)
    {
        //Finds the numbers in the file
        for (int i = 0; i < 3; i++)
        {
            Reputation[i] = data.Reputations[i];
        }
        Currency = data.Currency;
        //The UI will also then update
        UpdateRepLevel();
        UpdateMoney();
    }
    public void UpdateRepLevel()
    {
        //Loop through all the Factions and their reputations and runs through to get their level
        for (int i = 0; i < 3; i++)
        {
            switch (Reputation[i])
            {
                case < 100:
                    RepLevel[i] = 1;
                    break;
                case < 250:
                    RepLevel[i] = 2;
                    break;
                case < 600:
                    RepLevel[i] = 3;
                    break;
                case < 1250:
                    RepLevel[i] = 4;
                    break;
                case > 1250:
                    RepLevel[i] = 5;
                    break;
            }
        }
    }
    //This will keep the Money on screen consistant
    public void UpdateMoney()
    {
        GoldNum.text = Currency.ToString();
    }
}
