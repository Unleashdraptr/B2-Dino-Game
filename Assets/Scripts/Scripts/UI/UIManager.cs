using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    bool IsSaving;
    public DataManager data;
    public static bool Pause;
    public StatsStorage Stats;
    public QuestStorage Quests;
    public QuestSetup QuestList;
    public GameObject DeathScreen;

    public GameObject PauseMenu;
    public GameObject InventoryTab;
    public GameObject InfoTab;
    int CurrentMenu = 0;
    public GameObject[] DifferentMenus;
    public static bool InMap;
    public bool InInventory;
    public bool InInfo;

    public GameObject Shopui;
    public GameObject Bountyui;

    public RenderTexture[] AreaMap;
    public GameObject[] AreaMapCamera;
    public Slider Health;

    private void Start()
    {
        Pause = false;
        UpdateHealth((int)Health.value);
    }
    void Update()
    {

        //Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape) && Pause == true && ShopUI.InShop == false)
        {
            Pause = false;
            PauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Pause == false && ShopUI.InShop == false)
        {
            Pause = true;
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /*
        //Inventory Menu
        if(Input.GetKeyDown(KeyCode.Escape) && Pause == false && InInventory == true)
        {
            InventoryTab.SetActive(false);
            Pause = false;
            InInventory = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Input.GetKeyDown(KeyCode.E) && Pause == false && InInventory == false)
        {
            InventoryTab.SetActive(true);
            Pause = true;
            InInventory = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        */

        //Info tab UI
        if (Input.GetKeyDown(KeyCode.Escape) && Pause == false && InInfo == true)
        {
            InfoTab.SetActive(false);
            Pause = false;
            InInfo = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && Pause == false && InInfo == false)
        {
            InfoTab.SetActive(true);
            UpdateCurrentMenu();
            Pause = true;
            InInfo = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.Q) && Pause == true && InInfo == true)
        {
            CurrentMenu -= 1;
            if (CurrentMenu < 0)
            {
                CurrentMenu = DifferentMenus.Length - 1;
            }
            UpdateCurrentMenu();
        }
        if (Input.GetKeyDown(KeyCode.E) && Pause == true && InInfo == true)
        {
            CurrentMenu += 1;
            if (CurrentMenu > DifferentMenus.Length - 1)
            {
                CurrentMenu = 0;
            }
            UpdateCurrentMenu();
        }

        //Shop/ Bounty shop escape 
        if (Input.GetKeyDown(KeyCode.Escape) && ShopUI.InShop == true)
        {
            ShopUI.InShop = false;
            Shopui.transform.GetChild(0).gameObject.SetActive(false);
            Shopui.transform.GetChild(1).gameObject.SetActive(false);
            Pause = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            for (int i = 0; i < Shopui.transform.GetChild(1).childCount; i++)
            {
                if (Shopui.transform.GetChild(1).GetChild(i).childCount > 0)
                {
                    Destroy(Shopui.transform.GetChild(1).GetChild(i).GetChild(0).gameObject);
                    Shopui.transform.GetChild(1).GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
            Bountyui.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //Menu UI navigation 
    public void UpdateCurrentMenu()
    {
        for(int i = 0; i < DifferentMenus.Length; i++)
        {
            DifferentMenus[i].SetActive(false);
        }
        DifferentMenus[CurrentMenu].SetActive(true);
        if (CurrentMenu == 3)
        {
            InMap = true;
        }
        else if(CurrentMenu == 1)
        {
            UpdateFactionProgess();
        }
        else if(CurrentMenu == 2)
        {
            UpdateQuestsShown();
        }
        else
            InMap = false;
    }
    public void GoToCertainMenu(int MenuNum)
    {
        CurrentMenu = MenuNum;
        UpdateCurrentMenu();
    }
    public void HighlightMenu(Transform Menu)
    {
        Menu.GetChild(0).gameObject.SetActive(true);
    }
    public void UnHighlightMenu(Transform Menu)
    {
        Menu.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateFactionProgess()
    {
        for(int i = 0; i < 3; i++)
        {
            DifferentMenus[0].transform.GetChild(i + 1).GetComponent<Slider>().value = Stats.Reputation[i];
            DifferentMenus[0].transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text = Stats.RepLevel[i].ToString();
        }
    }
    public void UpdateQuestsShown()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform Quest = DifferentMenus[1].transform.GetChild(i + 1);
            int QuestID = Quests.CurQuests[i].QuestID;
            if (QuestList.FindQuest(QuestID) != null)
            {
                Quest.GetChild(0).gameObject.SetActive(true);
                if (QuestList.FindQuest(QuestID).Faction == 0)
                {
                    Quest.GetChild(0).GetComponent<Image>().color = Color.red;
                }
                else if (QuestList.FindQuest(QuestID).Faction == 1)
                {
                    Quest.GetChild(0).GetComponent<Image>().color = Color.blue;
                }
                else if (QuestList.FindQuest(QuestID).Faction == 2)
                {
                    Quest.GetChild(0).GetComponent<Image>().color = Color.green;
                }
                if (QuestList.FindQuest(QuestID).questType == ProductID.Quest.AREA)
                {
                    Quest.GetChild(2).gameObject.SetActive(true);
                    Vector3 Pos = new(QuestList.FindQuest(QuestID).SpawnPos.x, 1000, QuestList.FindQuest(QuestID).SpawnPos.z);
                    AreaMapCamera[i].transform.position = Pos;
                    Quest.GetChild(2).GetComponent<RawImage>().texture = AreaMap[i];
                }
                else
                {
                    Quest.GetChild(3).gameObject.SetActive(true);
                    Quest.GetChild(3).GetComponent<Image>().sprite = QuestList.FindQuest(QuestID).PreviewImage;
                }
                Quest.GetChild(4).GetComponent<TextMeshProUGUI>().text = QuestList.FindQuest(QuestID).QuestName;
                Quest.GetChild(5).GetComponent<TextMeshProUGUI>().text = QuestList.FindQuest(QuestID).Reward.ToString();
                Quest.GetChild(6).GetComponent<TextMeshProUGUI>().text = QuestList.FindQuest(QuestID).Dangerlevel.ToString();
            }
            else
            {
                Quest.GetChild(0).gameObject.SetActive(false);
                Quest.GetChild(2).gameObject.SetActive(false);
                Quest.GetChild(3).gameObject.SetActive(false);
                Quest.GetChild(4).GetComponent<TextMeshProUGUI>().text = " ";
                Quest.GetChild(5).GetComponent<TextMeshProUGUI>().text = " ";
                Quest.GetChild(6).GetComponent<TextMeshProUGUI>().text = " ";
            }
        }
    }
    public void DiscardQuest(int QuestInt)
    {
        Quests.RemoveQuest(Quests.CurQuests[QuestInt].QuestID);
        UpdateQuestsShown();
    }

    public void SaveLoadSlot(int SaveNum)
    {
        if (IsSaving == true)
        {
            data.SaveGame(SaveNum);
        }
        else
            data.LoadGame(SaveNum);
    }
    //Checking which slots have a save file and then loading from that
    public void LoadGame()
    {
        IsSaving = false;
        for (int i = 1; i < 5; i++)
        {
            if (!data.CheckLoad(0, i))
            {
                PauseMenu.transform.GetChild(i).GetComponent<Button>().interactable = false;
                PauseMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Save File";
            }
            else
                PauseMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i);
            if(i == DataManager.DataSlot)
            {
                PauseMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Current Save");
            }
        }
    }
    //Saving to any of the slots and letting the player pick which one
    public void SaveGame()
    {
        IsSaving = true;
        for (int i = 1; i < 5; i++)
        {
            PauseMenu.transform.GetChild(i).GetComponent<Button>().interactable = true;
            PauseMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i);
            if (i == DataManager.DataSlot)
            {
                PauseMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Current Save");
            }
        }
    }
    //Deleting another save that isnt the current one to avoid any in game issues
    public void DeleteSave()
    {
        //Delete code yet to be added
    }
    //Quitting to main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    //The stats UI that will run based of what stats are currently within the UI
    //Triggered when the player dies
    public void DeathUI()
    {
        DeathScreen.SetActive(true);
        Pause = true;
    }
    //Will run when health is changed to update the UI to be correct
    public void UpdateHealth(int hp)
    {
        Health.value = hp;
    }
}
