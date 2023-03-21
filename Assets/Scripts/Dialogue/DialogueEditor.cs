using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string dialogue;
    public bool HasMenu;
    public enum OptionType { SHOP };
    public OptionType[] menuCode;
    public string[] MenuName;
    public bool IsEnd;
}

public class DialogueEditor : MonoBehaviour
{
    public Dialogue[] dialogue;
}


