using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject Dialogue;
    public GameObject OptionDialogue;
    public Transform Options;

    FactionInfo Faction;
    public ShopUI Shop;
    public enum OptionType { SHOP };
    public OptionType menuCode;
    public int DialogueIterNum = -1;
    GameObject dialogueEditor;
    bool InDialogue;
    bool IsEnd;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InDialogue == true && IsEnd == false)
        {
            DialogueIterNum += 1;
            UpdateDialogue();
            Debug.Log("Next text");
            Debug.Log(DialogueIterNum);
        }
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && InDialogue == true)
        {
            InDialogue = false;
            OptionDialogue.SetActive(false);
            Dialogue.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameUIManager.Pause = false;
        }
    }
    public void StartDialogue(GameObject dia, FactionInfo Fact)
    {
        if (InDialogue == false)
        {
            Faction = Fact;
            dialogueEditor = dia;
            DialogueIterNum = 0;
            InDialogue = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameUIManager.Pause = true;
            Debug.Log("Setup Dialogue");
            Debug.Log(DialogueIterNum);
            UpdateDialogue();
        }
    }
    public void UpdateDialogue()
    {
        OptionDialogue.SetActive(false);
        Dialogue.SetActive(false);
        if (dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].HasMenu)
        {
            OptionDialogue.SetActive(true);
            OptionDialogue.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].dialogue;
            UpdateOptions();
        }
        else
        {
            Dialogue.SetActive(true);
            Dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].dialogue;
        }
        if(dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].IsEnd)
        {
            IsEnd = true;
        }
    }
    public void UpdateOptions()
    {
        int Seperation = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].MenuName.Length;
        for(int i = 0; i < Seperation; i++)
        {
            GameObject Option = Options.GetChild(i).gameObject;
            Option.SetActive(true);
            Option.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].MenuName[i];
        }
    }
    public void OptionPicked(int OptionNum)
    {
        switch (dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].menuCode[OptionNum])
        {
            case global::Dialogue.OptionType.SHOP:
                OptionDialogue.SetActive(false);
                Dialogue.SetActive(false);
                Shop.OpenFactionUI(Faction.LvlLimit, Faction.ItemsToSell, Faction.Faction);
                break;
        }
    }
}
