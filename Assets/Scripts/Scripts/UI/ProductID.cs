using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductID : MonoBehaviour
{
    public Sprite PreviewImage;
    public string Description;
    public int Cost;
    public int ItemID;

    public bool IsQuest;
    public enum Quest { NONE, PLANT, ANIMAL, ITEM, AREA, BOUNTY};
    public Quest quest;
    public int QuestID;
}
