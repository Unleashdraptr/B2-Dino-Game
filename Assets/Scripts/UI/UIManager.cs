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
    public GameObject PauseTint;
    public GameObject SaveMenu;
    public GameObject LoadMenu;
    public GameObject DataManger;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        Pause = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Pause == false)
        {
            Pause = true;
            GameMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            PauseTint.SetActive(true);
        }
    }
    public void ReturnToGame()
    {
        Pause = false;
        GameMenu.SetActive(false);
        PauseTint.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }    
    public void ReturnToGameMenu(string PrevMenu)
    {
        GameObject.Find(PrevMenu).SetActive(false);
        GameMenu.SetActive(true);
    }
    public void SaveGameProg()
    {
        GameMenu.SetActive(false);
        SaveMenu.SetActive(true);
    }
    public void LoadGameProg()
    {
        GameMenu.SetActive(false);
        LoadMenu.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (!DataManger.GetComponent<DataManager>().CheckLoad(0, i + 1))
            {
                LoadMenu.transform.GetChild(i).GetComponent<Button>().interactable = false;
                LoadMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Save File";
            }
            else
                LoadMenu.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ("Game " + i + 1);
        }
    }

}
