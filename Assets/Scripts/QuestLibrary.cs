using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestLibrary : MonoBehaviour
{
    public QuestStorage Queststorage;
    public StatsStorage Stats;


    public Quest[] AnimalQuests;
    public Bounty[] BountiesQuests;

    public void BountySetup(Bounty Bounties, Transform BountySlots)
    {
        if (Queststorage.QuestLimit == false)
        {
            if (BountySlots.GetChild(1).gameObject.activeInHierarchy)
            {
                Stats.Currency -= Bounties.Cost;
                Queststorage.CurBountys[0] = Bounties;
                Queststorage.UpdateCurQuests();
                GameObject Animal = Instantiate(Bounties.Animal, Bounties.SpawnPos, Quaternion.identity, GameObject.Find("AnimalsStorage").transform);
                Animal.GetComponent<IsTargetAnimal>().enabled = true;
            }
        }
    }
    public void AreaSelection(int QuestNum)
    {

    }

    public void ItemSelection(int QuestNum)
    {

    }

    public void SpeciesSelection(bool isPlant, int QuestNum)
    {

    }
}
[System.Serializable]
public class Quest
{
    public ProductID.Quest questType;
    [HideInInspector]
    public Sprite PreviewImage;
    [HideInInspector]
    public GameObject ToSpawn;
    [HideInInspector]
    [Range(0, 5)]
    public int Dangerlevel;

    public Vector3 SpawnPos;
    public int Cost;
    public int Reward;
    public string Desc;
    public bool Taken;
}

[CustomEditor(typeof(Quest))]
public class WaveUpdate : PropertyDrawer
{
    public Object source;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var effectRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var secondRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);

        var questType = property.FindPropertyRelative("questType");
        var previewImage = property.FindPropertyRelative("PreviewImage");
        var toSpawn = property.FindPropertyRelative("ToSpawn");
        var dangerLevel = property.FindPropertyRelative("Dangerlevel");

        questType.intValue = EditorGUI.Popup(effectRect, "Effect", questType.intValue, questType.enumNames);

        switch ((ProductID.Quest)questType.intValue)
        {
            case ProductID.Quest.PLANT:
                previewImage.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(Sprite), true);
                toSpawn.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(GameObject), true);
                break;
            case ProductID.Quest.ANIMAL:
                previewImage.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(Sprite), true);
                toSpawn.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(GameObject), true);
                dangerLevel.intValue = EditorGUILayout.IntSlider(dangerLevel.intValue, 0, 5);
                break;
            case ProductID.Quest.ITEM:
                previewImage.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(Sprite), true);
                toSpawn.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(GameObject), true);
                break;
            case ProductID.Quest.AREA:
                previewImage.exposedReferenceValue = EditorGUILayout.ObjectField("Preview Sprite", source, typeof(Sprite), true);
                break;
        }
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //This will need to be adjusted based on what you are displaying
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 2);
    }
}
[System.Serializable]
public class Bounty
{
    public Sprite AnimalImage;
    public GameObject Animal;
    public Vector3 SpawnPos;
    [Range(0, 5)]
    public int Dangerlevel;
    public int Cost;
    public int Reward;
    public string AnimalDesc;
    public bool Taken;
}

