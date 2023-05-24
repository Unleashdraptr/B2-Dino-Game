using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionRepSystem : MonoBehaviour, IDataHandler
{
    //Hunters, Merchants, Biologists, Explorers. there reputation number and the current level
    public int[] Reputation = new int[4];
    public int[] RepLevel = new int[4];
    public void SaveData(ref GameData data)
    {
        //Stores the numbers in the file
        for(int i = 0; i <4; i++)
        {
            data.Reputations[i] = Reputation[i];
        }
    }
    public void LoadData(GameData data)
    {
        //Finds the numbers in the file
        for (int i = 0; i < 4; i++)
        {
            Reputation[i] = data.Reputations[i];
        }
        UpdateRepLevel();
    }
    public void UpdateRepLevel()
    {
        //Loop through all the Factions and their reputations and runs through to get their level
        for(int i = 0; i < 4; i++)
        {
            switch(Reputation[i])
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
}
