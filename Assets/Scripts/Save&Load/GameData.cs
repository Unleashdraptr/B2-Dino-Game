using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    //Players stats that were last saved
    public Vector3 Position;
    public Quaternion Rotation;
    public float Stamina;
    public float Health;
    //Reputation among the factions
    public int[] Reputations = new int[4];
    //Player's money
    public int Currency;

}
