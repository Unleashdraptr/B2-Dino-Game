using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This is where the Dialogue options are used
public class DialogueManager : MonoBehaviour
{
    //It grabs the different menus that show, depending on what type of dialogue it is
    public GameObject Dialogue;
    public GameObject OptionDialogue;
    public Transform Options;

    //It also gets what faction type and the Shop UI for the shops
    FactionInfo Faction;
    public ShopUI Shop;
    public BountyListManager bounty;

    //This is where The dialogue is looped through
    public int DialogueIterNum = -1;
    GameObject dialogueEditor;

    //Some control bools
    bool InDialogue;
    bool IsEnd;
    public int CurrentBranch;
  
    private void Update()
    {
        //Checks if the player is entering the Dialogue
        if (Input.GetKeyDown(KeyCode.E) && InDialogue == true && IsEnd == false)
        {
            DialogueIterNum += 1;
            UpdateDialogue();
        }
    }
    void LateUpdate()
    {
        //If they are in Dialogue then leave it
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
        //This runs and sets up the Dialogue for the first text box
        if (InDialogue == false)
        {
            //Gives it the dialogue it is going to use
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
        //This is the same as the previous function just used for the shop versions
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
        //This is then used everytime the player will go to the next dialogue option
        Dialogue diag = dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum];
        //It resets both Dialogue UI options
        OptionDialogue.SetActive(false);
        Dialogue.SetActive(false);
        //It then checks the current branch
        if (diag.BranchNum == CurrentBranch)
        {
            //if its got a menu then run the Menu Dialogue UI
            if (diag.HasMenu)
            {
                OptionDialogue.SetActive(true);
                OptionDialogue.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = diag.dialogue;
                UpdateOptions();
            }
            //if its not then itll just run through the normal Dialogue UI
            else
            {
                Dialogue.SetActive(true);
                Dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = diag.dialogue;
            }
            //If its the end disable the player from going to the next one
            if (diag.IsEnd)
            {
                IsEnd = true;
            }
            //If its the end of the branch then reset to the first one
            if (diag.BranchEnd)
            {
                DialogueIterNum = 0;
                CurrentBranch = 0;
            }
        }
        //Else then go to the next dialogue
        else
        {
            DialogueIterNum += 1;
            UpdateDialogue();
        }
    }
    //If it is a menu then update each of the options to tell the player what each on does
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
    //This is attached to the buttons
    public void OptionPicked(int OptionNum)
    {
        //It checks what the options supposed to do out of these options, itll then run the code needed for it
        switch (dialogueEditor.GetComponent<DialogueEditor>().dialogue[DialogueIterNum].menuCode[OptionNum])
        {
            //For the shop
            case global::Dialogue.OptionType.SHOP:
                OptionDialogue.SetActive(false);
                Dialogue.SetActive(false);
                Shop.OpenFactionUI(Faction.LvlLimit, Faction.ItemsToSell, Faction.Faction);
                break;
            //if they want to exit the dialogue
            case global::Dialogue.OptionType.EXIT:
                OptionDialogue.SetActive(false);
                Dialogue.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UIManager.Pause = false;
                break;
            //If its going to branch to a different option
            case global::Dialogue.OptionType.BRANCH:
                CurrentBranch = OptionNum+1;
                UpdateDialogue();
                break;
            //Similar to the shop but for the Bounty system
            case global::Dialogue.OptionType.BOUNTYSHOP:
                OptionDialogue.SetActive(false);
                Dialogue.SetActive(false);
                bounty.OpenBountyList(Faction.LvlLimit[0]);
                break;
        }
    }
}
