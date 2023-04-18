using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : MonoBehaviour
{ 
    public GameObject MainScreen;
    public GameObject NewGameScreen;
    public GameObject LoadGameScreen;
    public void QuitButton()
    {
        Application.Quit();
    }
    public void NewGameSelect()
    {
        MainScreen.SetActive(false);
        NewGameScreen.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (GetComponent<DataManager>().CheckLoad(0, i+1))
            {
                NewGameScreen.transform.GetChild(i).GetComponent<Button>().interactable = false;
                NewGameScreen.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Taken");
            }
            else
                NewGameScreen.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "New Game";
        }
    }
    public void LoadGameSelect()
    {
        MainScreen.SetActive(false);
        LoadGameScreen.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (!GetComponent<DataManager>().CheckLoad(0, i + 1))
            {
                LoadGameScreen.transform.GetChild(i).GetComponent<Button>().interactable = false;
                LoadGameScreen.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Save File";
            }
            else
                LoadGameScreen.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i + 1);
        }
    }
    public void ReturnButton(string ScreenToActive)
    {
        GameObject.Find(ScreenToActive).SetActive(false);
        MainScreen.SetActive(true);

    }
    public void CreateNewGame(int SaveNum)
    {
        NewGameScreen.SetActive(false);
        GetComponent<DataManager>().NewGame();
        GetComponent<DataManager>().SaveGame(SaveNum);
        DataManager.DataSlot = SaveNum;
        SceneManager.LoadScene(1);
    }
    public void LoadGame(int SaveNum)
    {
        SceneManager.LoadScene(1);
        DataManager.DataSlot = SaveNum;
    }
}
