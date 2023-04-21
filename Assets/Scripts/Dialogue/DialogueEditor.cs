using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class DialogueEditor : MonoBehaviour
{
    public Dialogue[] dialogue;
}


