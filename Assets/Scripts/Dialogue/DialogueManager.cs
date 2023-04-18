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
    public int DialogueIterNum = -1;
    GameObject dialogueEditor;
    bool InDialogue;
    bool IsEnd;
    public int CurrentBranch;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InDialogue == true && IsEnd == false)
        {
            DialogueIterNum += 1;
            UpdateDialogue();
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
            UIManager.Pause = false;
        }
    }
    public void StartDialogue(GameObject dia)
    {
        if (InDialogue == false)
        {
            dialogueEditor = dia;
            DialogueIterNum = 0;
            CurrentBranch = 0;
            InDialogue = true;
            IsEnd = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Pause = true;
            UpdateDialogue();
        }
    }
    public void StartShopDialogue(GameObject dia, FactionInfo Fact)
    {
        if (InDialogue == false)
        {
            Faction = Fact;
            dialogueEditor = dia;
            DialogueIterNum = 0;
            CurrentBranch = 0;
            InDialogue = true;
            IsEnd = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Pause = true;
            UpdateDialogue();
        }
    }
    public void UpdateDialogue()
    {
        Dialogue diag = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum];
        OptionDialogue.SetActive(false);
        Dialogue.SetActive(false);
        if (diag.BranchNum == CurrentBranch)
        {
            if (diag.HasMenu)
            {
                OptionDialogue.SetActive(true);
                OptionDialogue.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = diag.dialogue;
                UpdateOptions();
            }
            else
            {
                Dialogue.SetActive(true);
                Dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = diag.dialogue;
            }
            if (diag.IsEnd)
            {
                IsEnd = true;
            }
            if (diag.BranchEnd)
            {
                DialogueIterNum = 0;
                CurrentBranch = 0;
            }
        }
        else
        {
            DialogueIterNum += 1;
            UpdateDialogue();
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
            case global::Dialogue.OptionType.EXIT:
                OptionDialogue.SetActive(false);
                Dialogue.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UIManager.Pause = false;
                break;
            case global::Dialogue.OptionType.BRANCH:
                CurrentBranch = OptionNum+1;
                UpdateDialogue();
                break;
        }
    }
}
