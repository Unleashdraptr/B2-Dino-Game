using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public string FileName;
    public int Hrs;
    public string LastPlayed;


    //Players stats that were last saved
    public Vector3 Position;
    public Quaternion Rotation;
    public float Stamina;
    public float Health;
    public int Wealth;
    //Reputation among the factions
    public int[] Reputations = new int[3];
    //Player's money
    public int Currency;
}
