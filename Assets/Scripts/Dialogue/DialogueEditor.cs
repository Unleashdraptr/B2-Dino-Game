using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a class used to store data about the dialogue currently displayed
[System.Serializable]
public class Dialogue
{
    public string dialogue;
    public bool HasMenu;
    public enum OptionType { SHOP, EXIT, BOUNTYSHOP, BRANCH };
    public OptionType[] menuCode;
    public string[] MenuName;
    public bool BranchEnd;
    public bool IsEnd;
    public int BranchNum;
}

//This is where i can then store the dialogue and change the options
public class DialogueEditor : MonoBehaviour
{
    public Dialogue[] dialogue;
}


