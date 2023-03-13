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
    public int Currency;
    //Reputation among the factions
    public int[] Reputations = new int[3];

    public bool[] HasItem = new bool[8];
    public int[] ItemPrefabID = new int[8];
    public string[] itemName = new string[8];
    public int[] FactionID = new int[8];
    public int[] sellPrice = new int[8];
    public int[] RepValue = new int[8];

}
