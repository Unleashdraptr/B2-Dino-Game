using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject DeathScreen;
    public GameObject GameMenu;
    public GameObject PauseTint;
    public GameObject SaveMenu;
    public GameObject LoadMenu;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            PauseTint.SetActive(true);
        }
    }
    public void ReturnToGame()
    {
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
    }

}
