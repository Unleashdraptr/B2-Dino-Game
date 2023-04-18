using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static bool Pause;

    public GameObject DeathScreen;

    public GameObject GameMenu;
    int CurrentMenu = 0;
    public GameObject[] DifferentMenus;

    public Slider Health;

    private void Start()
    {
        Pause = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Pause == true && ShopUI.InShop == false)
        {
            ResumeGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Pause == false && ShopUI.InShop == false)
        {
            Pause = true;
            GameMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.Q) && Pause == true)
        {
            CurrentMenu -= 1;
            if (CurrentMenu < 0)
            {
                CurrentMenu = DifferentMenus.Length - 1;
            }
            UpdateCurrentMenu();
        }
        if (Input.GetKeyDown(KeyCode.E) && Pause == true)
        {
            CurrentMenu += 1;
            if(CurrentMenu > DifferentMenus.Length - 1)
            {
                CurrentMenu = 0;
            }
            UpdateCurrentMenu();
        }
    }
    public void ResumeGame()
    {
        Pause = false;
        GameMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Menu UI navigation 
    public void UpdateCurrentMenu()
    {
        for(int i = 0; i < DifferentMenus.Length; i++)
        {
            DifferentMenus[i].SetActive(false);
        }
        DifferentMenus[CurrentMenu].SetActive(true);
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
    //Menu UI 1-3s code to load the faction info the player will need to understand where they stand with each of them



    //Menu UI 4s code to load the map and how what the player has currently explored with their waypoints and Quest specific knowledge



    //Menu UI 5s code to load what the player currently has in their inventory and let them see more detail about each of the items



    //Menu UI 6s code to update the info to the correct animal



    //Menu UI 7s code for each of the save/load functions
    //Checking which slots have a save file and then loading from that
    public void LoadGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!GetComponent<DataManager>().CheckLoad(0, i + 1))
            {
                DifferentMenus[6].transform.GetChild(i).GetComponent<Button>().interactable = false;
                DifferentMenus[6].transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Save File";
            }
            else
                DifferentMenus[6].transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i + 1);
            if(i + 1 == DataManager.DataSlot)
            {
                DifferentMenus[6].transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Current Save");
            }
        }
    }
    //Saving to any of the slots and letting the player pick which one
    public void SaveGame()
    {
        for (int i = 0; i < 4; i++)
        {
            DifferentMenus[6].transform.GetChild(i).GetComponent<Button>().interactable = true;
            DifferentMenus[6].transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i + 1);
            if (i + 1 == DataManager.DataSlot)
            {
                DifferentMenus[6].transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Current Save");
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
